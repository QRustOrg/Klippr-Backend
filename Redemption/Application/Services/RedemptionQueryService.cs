using Klippr_Backend.Redemption.Domain.Queries;
using Klippr_Backend.Redemption.Domain.Repositories;
using Klippr_Backend.Redemption.Domain.Services;
using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Redemption.Application.Services;

/// <summary>
/// Implementa los casos de uso de lectura para el bounded context de canjes.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Delega directamente al repositorio sin transformar los datos; la conversion ocurre en la capa de interfaz.
/// </remarks>
public class RedemptionQueryService(IRedemptionRepository redemptionRepository) : IRedemptionQueryService
{
    /// <inheritdoc />
    public Task<RedemptionAggregate?> Handle(GetRedemptionByIdQuery query)
    {
        return redemptionRepository.FindByIdAsync(query.RedemptionId);
    }

    /// <inheritdoc />
    public Task<IEnumerable<RedemptionAggregate>> Handle(GetRedemptionsByConsumerIdQuery query)
    {
        return redemptionRepository.FindByConsumerIdAsync(query.ConsumerId);
    }

    /// <inheritdoc />
    public Task<IEnumerable<RedemptionAggregate>> Handle(GetRedemptionsByBusinessIdQuery query)
    {
        return redemptionRepository.FindByBusinessIdAsync(query.BusinessId);
    }
}
