using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.SharedKernel;
using ShoppingCart.Products.Application.Interfaces;
using ShoppingCart.Products.Domain.Entities;
using ShoppingCart.Products.Infrastructure.Data;
using ShoppingCart.Products.Infrastructure.Services;

namespace ShoppingCart.Products.Infrastructure;

public static class DependencyContainer
{
    public static IServiceCollection ResolveProductsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Resolved shared kernel
        services.AddSharedKernel();

        // Resolved database
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ProductsDbContext>(options => options.UseSqlServer(connectionString));

        // Resolved repositories
        services.AddEntityRepository<Product, Guid, ProductsDbContext>();

        // Resolved services
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
