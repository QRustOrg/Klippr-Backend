using Klippr_Backend.Redemption.Domain.Queries;
using Klippr_Backend.Redemption.Domain.Services;
using Klippr_Backend.Redemption.Interface.Assemblers;
using Klippr_Backend.Redemption.Interface.Resources;

namespace Klippr_Backend.Redemption.Interface.Facade;

/// <summary>
/// Fachada que expone operaciones de lectura del bounded context de canjes a otros contextos.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Solo expone consultas para proteger la invariante del agregado frente a accesos externos.
/// Retorna recursos en lugar de agregados para evitar acoplar otros contextos al modelo de dominio.
/// </remarks>
public class RedemptionContextFacade(IRedemptionQueryService redemptionQueryService)
{
    /// <summary>
    /// Obtiene un canje por su identificador.
    /// </summary>
    /// <param name="redemptionId">Identificador del canje consultado.</param>
    /// <returns>Recurso del canje o <see langword="null"/> si no existe.</returns>
    public async Task<RedemptionResource?> GetByIdAsync(int redemptionId)
    {
        var redemption = await redemptionQueryService
            .Handle(new GetRedemptionByIdQuery(redemptionId))
            .ConfigureAwait(false);

        return redemption is null
            ? null
            : RedemptionResourceFromEntityAssembler.ToResource(redemption);
    }

    /// <summary>
    /// Obtiene los canjes asociados a un consumidor.
    /// </summary>
    /// <param name="consumerId">Identificador del consumidor consultado.</param>
    /// <returns>Coleccion de canjes del consumidor.</returns>
    public async Task<IReadOnlyList<RedemptionResource>> GetByConsumerIdAsync(Guid consumerId)
    {
        var redemptions = await redemptionQueryService
            .Handle(new GetRedemptionsByConsumerIdQuery(consumerId))
            .ConfigureAwait(false);

        return RedemptionResourceFromEntityAssembler.ToResources(redemptions);
    }

    /// <summary>
    /// Obtiene los canjes asociados a un negocio.
    /// </summary>
    /// <param name="businessId">Identificador del negocio consultado.</param>
    /// <returns>Coleccion de canjes del negocio.</returns>
    public async Task<IReadOnlyList<RedemptionResource>> GetByBusinessIdAsync(Guid businessId)
    {
        var redemptions = await redemptionQueryService
            .Handle(new GetRedemptionsByBusinessIdQuery(businessId))
            .ConfigureAwait(false);

        return RedemptionResourceFromEntityAssembler.ToResources(redemptions);
    }
}
