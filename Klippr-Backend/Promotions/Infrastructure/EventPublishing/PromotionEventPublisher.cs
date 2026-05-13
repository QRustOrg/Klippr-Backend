using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Shared.Infrastructure.EventPublishing;

namespace Klippr_Backend.Promotions.Infrastructure.EventPublishing;

/// <summary>
/// Publica los eventos de dominio pendientes generados por promociones.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// El publicador coordina el despacho de eventos fuera del agregado para mantener
/// separada la logica de dominio del mecanismo de publicacion.
/// </remarks>
public class PromotionEventPublisher(IDomainEventDispatcher domainEventDispatcher)
{
    /// <summary>
    /// Publica los eventos pendientes de una coleccion de promociones.
    /// </summary>
    /// <param name="promotions">Promociones que pueden contener eventos pendientes.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <remarks>
    /// Los eventos se limpian del agregado solo despues de haber sido despachados correctamente.
    /// </remarks>
    public async Task PublishAsync(
        IEnumerable<Promotion> promotions,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(promotions);

        foreach (var promotion in promotions)
        {
            var domainEvents = promotion.DomainEvents.ToArray();

            foreach (var domainEvent in domainEvents)
            {
                await domainEventDispatcher
                    .DispatchAsync(domainEvent, cancellationToken)
                    .ConfigureAwait(false);
            }

            promotion.ClearDomainEvents();
        }
    }
}
