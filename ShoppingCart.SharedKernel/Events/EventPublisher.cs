using MediatR;
using ShoppingCart.SharedKerel.Extensions;

namespace ShoppingCart.SharedKerel.Events;

internal class EventPublisher : IEventPublisher
{
    private readonly IPublisher _publisher;

    public EventPublisher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public void Publish(IEnumerable<IHaveDomainEvents> entitiesWithDomainEvents)
    {
        foreach (var entity in entitiesWithDomainEvents)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearDoaminEvents();

            foreach (var @event in events)
            {
                Publish(@event);
            }
        }

        throw new NotImplementedException();
    }

    private void Publish<T>(T @event) where T : IEvent
    {
        Task.Run(() => _publisher.Publish(@event)).Forget();
    }
}