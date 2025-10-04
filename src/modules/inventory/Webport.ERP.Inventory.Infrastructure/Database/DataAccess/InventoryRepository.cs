using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Webport.ERP.Inventory.Application.Data;

namespace Webport.ERP.Inventory.Infrastructure.Database.DataAccess;

public class InventoryRepository<TEntity> : IInventoryRepository<TEntity>
    where TEntity : class
{
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public InventoryRepository(InventoryDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    // Create
    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    // Read 
    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    // Update

    public void Update(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    // Delete

    public void Delete(TEntity? entity)
    {
        _dbSet.Remove(entity!);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public TEntity FindOne(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null, CancellationToken cancellationToken = default)
    {
        return Get(findOptions).FirstOrDefault(predicate)!;
    }

    public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    private DbSet<TEntity> Get(FindOptions? findOptions = null)
    {
        findOptions ??= new FindOptions();

        DbSet<TEntity> entity = _dbContext.Set<TEntity>();

        if (findOptions.IsAsNoTracking && findOptions.IsIgnoreAutoIncludes)
        {
            entity.IgnoreAutoIncludes().AsNoTracking();
        }
        else if (findOptions.IsIgnoreAutoIncludes)
        {
            entity.IgnoreAutoIncludes();
        }
        else if (findOptions.IsAsNoTracking)
        {
            entity.AsNoTracking();
        }

        return entity;
    }
}