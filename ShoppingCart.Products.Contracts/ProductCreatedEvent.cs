using ShoppingCart.SharedKernel.Events;

namespace ShoppingCart.Products.Contracts
{
    public record ProductCreatedEvent(Guid Id, string Title, string Description, decimal Price) : IEvent;
}
