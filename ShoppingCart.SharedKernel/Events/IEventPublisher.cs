namespace ShoppingCart.SharedKerel.Events;

public interface IEventPublisher
{
    void Publish(IEnumerable<IHaveDomainEvents> entitiesWithDomainEvents);
}
