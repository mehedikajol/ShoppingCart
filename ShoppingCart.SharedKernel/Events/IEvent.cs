using MediatR;

namespace ShoppingCart.SharedKerel.Events;

public interface IEvent : INotification
{
}

public interface IHaveDomainEvents
{
    IReadOnlyCollection<IEvent> DomainEvents { get; }
    void ClearDoaminEvents();
}