using Microsoft.EntityFrameworkCore;
using ShoppingCart.Products.Domain.Entities;
using ShoppingCart.SharedKerel.Entities;
using ShoppingCart.SharedKernel.Events;

namespace ShoppingCart.Products.Infrastructure.Data;

internal class ProductsDbContext : DbContext
{
    private readonly IEventDispatcher _eventDispatcher;

    public ProductsDbContext(DbContextOptions<ProductsDbContext> options, IEventDispatcher eventDispatcher)
        : base(options)
    {
        _eventDispatcher = eventDispatcher;
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Product");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>()
            .HavePrecision(18, 6);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = Guid.NewGuid();
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = Guid.NewGuid();
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                default:
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        var entitiesWithEvents = ChangeTracker.Entries<IHaveDomainEvents>()
            .Select(p => p.Entity)
            .Where(p => p.DomainEvents.Any())
            .ToArray();

        _eventDispatcher.Publish(entitiesWithEvents);

        return result;
    }
}
