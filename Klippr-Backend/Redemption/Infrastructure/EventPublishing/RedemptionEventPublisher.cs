using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Redemption.Infrastructure.EventPublishing;

/// <summary>
/// Publicador inicial de eventos del bounded context de canjes.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Esta implementacion minima reserva el punto de integracion para mensajeria real
/// cuando Redemption publique eventos hacia otros bounded contexts, como Analytics.
/// </remarks>
public class RedemptionEventPublisher
{
    /// <summary>
    /// Publica la generacion de un nuevo canje.
    /// </summary>
    /// <param name="redemption">Canje generado.</param>
    public Task PublishRedemptionGeneratedAsync(RedemptionAggregate redemption)
    {
        ArgumentNullException.ThrowIfNull(redemption);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Publica la confirmacion de uso de una promocion.
    /// </summary>
    /// <param name="redemption">Canje confirmado.</param>
    public Task PublishPromotionRedeemedAsync(RedemptionAggregate redemption)
    {
        ArgumentNullException.ThrowIfNull(redemption);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Publica la expiracion de un canje.
    /// </summary>
    /// <param name="redemption">Canje expirado.</param>
    public Task PublishRedemptionExpiredAsync(RedemptionAggregate redemption)
    {
        ArgumentNullException.ThrowIfNull(redemption);

        return Task.CompletedTask;
    }
}
