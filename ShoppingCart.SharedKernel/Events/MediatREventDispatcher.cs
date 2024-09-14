using MediatR;
using ShoppingCart.SharedKerel.Extensions;

namespace ShoppingCart.SharedKernel.Events;

internal class MediatREventDispatcher : IEventDispatcher
{
    private readonly IPublisher _publisher;

    public MediatREventDispatcher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public void Publish(IEnumerable<IHaveDomainEvents> entitiesWithDomainEvents)
    {
        foreach (var entity in entitiesWithDomainEvents)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();

            foreach (var @event in events)
            {
                Publish(@event);
            }
        }

        throw new NotImplementedException();
    }

    public void Publish(IEvent @event)
    {
        Task.Run(() => _publisher.Publish(@event)).Forget();
    }
}