namespace Webport.ERP.Inventory.Application.Interfaces;

public interface IInventoryRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
}