using ShoppingCart.SharedKerel.Events;

namespace ShoppingCart.SharedKernel.Events;

public interface IHaveDomainEvents
{
    IReadOnlyCollection<IEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
