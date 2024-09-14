using ShoppingCart.SharedKernel.Events;

namespace ShoppingCart.Products.Contracts
{
    public record ProductDeletedEvent(Guid Id) : IEvent;
}
