using ShoppingCart.SharedKerel.Entities;
using ShoppingCart.SharedKernel.Events;
using ShoppingCart.SharedKernel.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Products.Domain.Entities;

public sealed class Product : AuditableEntity, IHaveDomainEvents
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }

    [NotMapped]
    public IReadOnlyCollection<IEvent> DomainEvents => _events;
    private List<IEvent> _events = new();
    public void ClearDomainEvents() => _events.Clear();
    private void AddDomainEvent(IEvent @event) => _events.Add(@event);

    public static Product Create(string title, string description, decimal price)
    {
        var product = new Product
        {
            Title = title,
            Description = description,
            Price = price
        };
        return product;
    }

    public void Update(string title, string description, decimal price)
    {
        Title = title;
        Description = description;
        Price = price;
    }

    #region Validation
    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainValidationException("Product title is null or empty");
        }

        if (title.Length < 5)
        {
            throw new DomainValidationException("Product title is too short, atleast 5 characters");
        }

        Title = title;
    }

    private void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainValidationException("Product description is null or empty");
        }

        if (description.Length < 15)
        {
            throw new DomainValidationException("Product description is too short, aleast 15 characters");
        }

        Description = description;
    }

    private void SetPrice(decimal price)
    {
        if (price < 0)
        {
            throw new DomainValidationException("Product price cannot be negative");
        }

        Price = price;
    }
    #endregion
}
