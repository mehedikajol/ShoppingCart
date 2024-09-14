using ShoppingCart.SharedKernel.Events;

namespace ShoppingCart.Products.Contracts
{
    public record ProductRemovedEvent(Guid Id) : IEvent;
}
