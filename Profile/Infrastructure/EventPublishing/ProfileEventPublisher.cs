using Klippr_Backend.Profile.Application.OutboundServices;
using Klippr_Backend.Profile.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Klippr_Backend.Profile.Infrastructure.EventPublishing;

public class ProfileEventPublisher : IEventPublisher
{
    private readonly ILogger<ProfileEventPublisher> _logger;

    public ProfileEventPublisher(ILogger<ProfileEventPublisher> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        if (domainEvent == null)
            throw new ArgumentNullException(nameof(domainEvent));

        try
        {
            _logger.LogInformation($"Publishing event: {domainEvent.GetType().Name}");

            // In production, this would integrate with message brokers like RabbitMQ, Azure Service Bus, etc.
            // For now, we log the event and could persist to a table for eventual consistency

            await Task.Delay(0, cancellationToken); // Placeholder for async operation

            _logger.LogInformation($"Event published successfully: {domainEvent.GetType().Name}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to publish event: {domainEvent.GetType().Name} - {ex.Message}");
            throw;
        }
    }

    public async Task PublishMultipleAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default)
    {
        if (events == null)
            throw new ArgumentNullException(nameof(events));

        try
        {
            var eventsList = events.ToList();
            _logger.LogInformation($"Publishing {eventsList.Count} events");

            var tasks = eventsList.Select(evt => PublishAsync(evt, cancellationToken));
            await Task.WhenAll(tasks);

            _logger.LogInformation($"All {eventsList.Count} events published successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to publish multiple events: {ex.Message}");
            throw;
        }
    }
}
