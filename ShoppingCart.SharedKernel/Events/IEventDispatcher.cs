namespace ShoppingCart.SharedKernel.Events;

public interface IEventDispatcher
{
    void Publish(IEnumerable<IHaveDomainEvents> entitiesWithDomainEvents);
    void Publish(IEvent @event);
}
