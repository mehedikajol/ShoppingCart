using ShoppingCart.SharedKernel.Events;

namespace ShoppingCart.Products.Contracts
{
    public record ProductUpdatedEvent(Guid Id, string Title, string Description, decimal Price) : IEvent;
}
