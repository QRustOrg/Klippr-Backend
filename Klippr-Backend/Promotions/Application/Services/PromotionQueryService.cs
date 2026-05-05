using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Promotions.Domain.Queries;
using Klippr_Backend.Promotions.Domain.Repositories;
using Klippr_Backend.Promotions.Domain.Services;

namespace Klippr_Backend.Promotions.Application.Services;

/// <summary>
/// Implementa los casos de uso de lectura para promociones.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Este servicio delega la obtencion de datos al repositorio y mantiene la capa de interfaz desacoplada de los detalles de persistencia.
/// </remarks>
public class PromotionQueryService(IPromotionRepository promotionRepository) : IPromotionQueryService
{
    /// <inheritdoc />
    public Task<Promotion?> GetByIdAsync(
        GetPromotionByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return promotionRepository.GetByIdAsync(query.PromotionId, cancellationToken);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<Promotion>> GetByBusinessIdAsync(
        GetPromotionsByBusinessIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return promotionRepository.GetByBusinessIdAsync(query.BusinessId, cancellationToken);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<Promotion>> GetActiveAsync(
        GetActivePromotionsQuery query,
        CancellationToken cancellationToken = default)
    {
        return promotionRepository.GetActiveAsync(cancellationToken);
    }
}
