using Klippr_Backend.Shared.Domain.Events;

namespace Klippr_Backend.Promotions.Domain.Events;

/// <summary>
/// Evento de dominio emitido cuando una promoción cambia a estado publicado.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="PromotionId">Identificador único de la promoción publicada.</param>
/// <param name="BusinessId">Identificador del negocio propietario de la promoción.</param>
/// <remarks>
/// El evento permite que otros componentes reaccionen a la publicación sin acoplarse
/// directamente al agregado de promociones.
/// </remarks>
public record PromotionPublishedEvent(Guid PromotionId, Guid BusinessId) : IDomainEvent
{
    /// <inheritdoc />
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
