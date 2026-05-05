using Klippr_Backend.Promotions.Domain.Queries;
using Klippr_Backend.Promotions.Domain.Services;
using Klippr_Backend.Promotions.Interface.Assemblers;
using Klippr_Backend.Promotions.Interface.Resources;

namespace Klippr_Backend.Promotions.Interface.Facade;

/// <summary>
/// Expone operaciones de consulta del contexto de promociones para otros bounded contexts.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// La fachada evita que otros contextos dependan directamente de agregados o repositorios
/// internos de Promotions.
/// </remarks>
public class PromotionsContextFacade(IPromotionQueryService promotionQueryService)
{
    /// <summary>
    /// Obtiene una promocion por su identificador.
    /// </summary>
    /// <param name="promotionId">Identificador unico de la promocion consultada.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Recurso de promocion o <see langword="null"/> si no existe.</returns>
    public async Task<PromotionResource?> GetByIdAsync(
        Guid promotionId,
        CancellationToken cancellationToken = default)
    {
        var promotion = await promotionQueryService
            .GetByIdAsync(new GetPromotionByIdQuery(promotionId), cancellationToken)
            .ConfigureAwait(false);

        return promotion is null
            ? null
            : PromotionResourceFromEntityAssembler.ToResource(promotion);
    }

    /// <summary>
    /// Obtiene promociones activas disponibles para consumidores.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Coleccion de recursos de promociones activas.</returns>
    public async Task<IReadOnlyList<PromotionResource>> GetActiveAsync(
        CancellationToken cancellationToken = default)
    {
        var promotions = await promotionQueryService
            .GetActiveAsync(new GetActivePromotionsQuery(), cancellationToken)
            .ConfigureAwait(false);

        return PromotionResourceFromEntityAssembler.ToResources(promotions);
    }

    /// <summary>
    /// Obtiene promociones asociadas a un negocio.
    /// </summary>
    /// <param name="businessId">Identificador del negocio propietario.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Coleccion de recursos de promociones del negocio.</returns>
    public async Task<IReadOnlyList<PromotionResource>> GetByBusinessIdAsync(
        Guid businessId,
        CancellationToken cancellationToken = default)
    {
        var promotions = await promotionQueryService
            .GetByBusinessIdAsync(new GetPromotionsByBusinessIdQuery(businessId), cancellationToken)
            .ConfigureAwait(false);

        return PromotionResourceFromEntityAssembler.ToResources(promotions);
    }
}
