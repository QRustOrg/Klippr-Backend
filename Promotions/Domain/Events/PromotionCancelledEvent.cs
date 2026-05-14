using Klippr_Backend.Shared.Domain.Events;

namespace Klippr_Backend.Promotions.Domain.Events;

/// <summary>
/// Evento de dominio emitido cuando una promoción es cancelada.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="PromotionId">Identificador único de la promoción cancelada.</param>
/// <param name="BusinessId">Identificador del negocio propietario de la promoción.</param>
/// <remarks>
/// El evento comunica la cancelación a otros componentes sin mover la lógica de negocio
/// fuera del agregado de promociones.
/// </remarks>
public record PromotionCancelledEvent(Guid PromotionId, Guid BusinessId) : IDomainEvent
{
    /// <inheritdoc />
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
