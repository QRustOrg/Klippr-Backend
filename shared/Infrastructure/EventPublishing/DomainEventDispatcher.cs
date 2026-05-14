using Klippr_Backend.Shared.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Klippr_Backend.Shared.Infrastructure.EventPublishing;

/// <summary>
/// Despacha eventos de dominio usando los handlers registrados en el contenedor de dependencias.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// La resolucion se realiza por el tipo concreto del evento para permitir handlers
/// fuertemente tipados sin acoplar el dominio a infraestructura.
/// </remarks>
public class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    /// <inheritdoc />
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        var handlers = serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var handleMethod = handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync));

            if (handleMethod?.Invoke(handler, [domainEvent, cancellationToken]) is Task task)
                await task.ConfigureAwait(false);
        }
    }
}
