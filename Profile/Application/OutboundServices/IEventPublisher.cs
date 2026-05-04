using Domain.Events;

namespace Application.OutboundServices;

public interface IEventPublisher
{
    Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken = default);
    Task PublishMultipleAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default);
}
