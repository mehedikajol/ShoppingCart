using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.SharedKerel.Entities;
using ShoppingCart.SharedKerel.Repositories;
using ShoppingCart.SharedKernel.Events;

namespace OnlineShop.SharedKernel;

public static class DependencyResolver
{
    public static IServiceCollection AddSharedKernel(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyResolver).Assembly);
        });

        services.AddScoped<IEventDispatcher, MediatREventDispatcher>();

        return services;
    }

    public static IServiceCollection AddEntityRepository<TEntity, TId, TContext>(this IServiceCollection services)
        where TEntity : class, IEntity<TId>
        where TContext : DbContext
    {
        return services.AddScoped<IRepository<TEntity, TId>, Repository<TEntity, TId, TContext>>();
    }
}
