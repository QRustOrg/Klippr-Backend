using Klippr_Backend.Profile.Domain.Events;

namespace Klippr_Backend.Profile.Application.OutboundServices;

public interface IEventPublisher
{
    Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken = default);
    Task PublishMultipleAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default);
}
