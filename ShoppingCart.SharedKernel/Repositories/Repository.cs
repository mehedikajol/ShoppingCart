using Microsoft.EntityFrameworkCore;
using ShoppingCart.SharedKerel.Entities;
using ShoppingCart.SharedKerel.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ShoppingCart.SharedKerel.Repositories;

internal class Repository<TEntity, TId, TContext> : IRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TContext : DbContext
{
    protected readonly TContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(TContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<TEntity?> GetAsync(TId id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<PaginatedData<TEntity>> GetPaginatedDataAsync(PaginatedRequest request)
    {
        var result = new PaginatedData<TEntity>();

        var query = _dbSet.AsQueryable();

        // handle navigation properties
        if (request.NavigationProperties != null && request.NavigationProperties.Any())
        {
            query = IncludeChildProperties(query, request.NavigationProperties);
        }

        // handle search feature
        if (!string.IsNullOrWhiteSpace(request.SearchText) && !string.IsNullOrWhiteSpace(request.SearchProperty))
        {
            query = FilterByProperty(query, request.SearchProperty, request.SearchText);
        }

        // handler ordering
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortOrder > 0 ? query.OrderBy(request.SortBy) : query.OrderBy(request.SortBy + " desc");
        }

        result.TotalItemsCount = query.Count();

        query = query
            .Skip((request.CurrentPage - 1) * request.ItemsPerPage)
            .Take(request.ItemsPerPage);

        result.TotalPages = (int)Math.Ceiling((double)result.TotalItemsCount / request.ItemsPerPage);
        result.CurrentPage = request.CurrentPage;
        result.ItemsPerPage = request.ItemsPerPage;
        result.Items = await query.ToListAsync();
        result.CurrentItemsCount = result.Items.Count;

        return result;
    }

    public async Task CreateAsync(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity is null)
        {
            return;
        }

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private IQueryable<T> IncludeChildProperties<T>(IQueryable<T> query, params string[] navigationProperties)
    {
        var entityType = typeof(T);
        foreach (var navigationProperty in navigationProperties)
        {
            var properties = navigationProperty.Split('.');
            var parameter = Expression.Parameter(entityType, "x");
            Expression property = parameter;

            Type currentType = entityType;

            for (int i = 0; i < properties.Length; i++)
            {
                var propertyInfo = currentType.GetProperty(properties[i]);
                if (propertyInfo == null) throw new ArgumentException($"Property '{properties[i]}' not found on type '{currentType.Name}'");

                property = Expression.Property(property, propertyInfo);
                currentType = propertyInfo.PropertyType;

                // Handle collections for ThenInclude
                if (typeof(IEnumerable<>).IsAssignableFrom(currentType) && currentType.IsGenericType)
                {
                    currentType = currentType.GetGenericArguments()[0];
                }
            }

            var lambda = Expression.Lambda(property, parameter);

            var includeMethod = typeof(EntityFrameworkQueryableExtensions).GetMethods()
                .First(m => m.Name == "Include" && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityType, lambda.Body.Type);

            query = (IQueryable<T>)includeMethod.Invoke(null, new object[] { query, lambda })!;
        }
        return query;
    }

    private IQueryable<T> FilterByProperty<T>(IQueryable<T> query, string propertyName, string searchText)
    {
        if (string.IsNullOrWhiteSpace(propertyName) || string.IsNullOrEmpty(searchText))
        {
            return query;
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var propertyToLower = Expression.Call(property, "ToLower", null);
        var searchTextToLower = Expression.Constant(searchText.ToLower());
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var containsExpression = Expression.Call(propertyToLower, containsMethod!, searchTextToLower);
        var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

        return query.Where(lambda);
    }

}
