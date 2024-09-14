using ShoppingCart.SharedKerel.Entities;
using ShoppingCart.SharedKerel.Models;
using System.Linq.Expressions;

namespace ShoppingCart.SharedKerel.Repositories;

public interface IRepository<TEntity, TId>
 where TEntity : class, IEntity<TId>
{
    Task<IReadOnlyCollection<TEntity>> GetAllAsync();
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
    Task<PaginatedData<TEntity>> GetPaginatedDataAsync(PaginatedRequest request);
    Task<TEntity?> GetAsync(TId id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
}
