using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;
using Klippr_Backend.Redemption.Domain.Queries;

namespace Klippr_Backend.Redemption.Domain.Services;

/// <summary>
/// Define las operaciones de lectura disponibles para consultar canjes.
/// </summary>
/// <author>Samuel Bonifacio</author>
public interface IRedemptionQueryService
{
    /// <summary>
    /// Obtiene un canje por su identificador.
    /// </summary>
    /// <param name="query">Query con el identificador del canje.</param>
    /// <returns>Canje encontrado o <see langword="null"/> si no existe.</returns>
    Task<RedemptionAggregate?> Handle(GetRedemptionByIdQuery query);

    /// <summary>
    /// Obtiene los canjes asociados a un consumidor.
    /// </summary>
    /// <param name="query">Query con el identificador del consumidor.</param>
    /// <returns>Coleccion de canjes del consumidor indicado.</returns>
    Task<IEnumerable<RedemptionAggregate>> Handle(GetRedemptionsByConsumerIdQuery query);

    /// <summary>
    /// Obtiene los canjes asociados a un negocio.
    /// </summary>
    /// <param name="query">Query con el identificador del negocio.</param>
    /// <returns>Coleccion de canjes del negocio indicado.</returns>
    Task<IEnumerable<RedemptionAggregate>> Handle(GetRedemptionsByBusinessIdQuery query);
}
