namespace Webport.ERP.Identity.Application.Interfaces;

public interface IIdentityRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
}