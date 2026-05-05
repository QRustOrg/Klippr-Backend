using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Promotions.Interface.Resources;

namespace Klippr_Backend.Promotions.Interface.Assemblers;

/// <summary>
/// Convierte agregados de promociones en recursos de respuesta.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// La conversion expone valores simples apropiados para HTTP sin filtrar detalles internos
/// del agregado como eventos de dominio pendientes.
/// </remarks>
public static class PromotionResourceFromEntityAssembler
{
    /// <summary>
    /// Convierte una promocion en un recurso de respuesta.
    /// </summary>
    /// <param name="promotion">Agregado de promocion que sera expuesto.</param>
    /// <returns>Recurso de respuesta de promocion.</returns>
    public static PromotionResource ToResource(Promotion promotion)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        return new PromotionResource(
            promotion.Id,
            promotion.BusinessId,
            promotion.Title,
            promotion.Description,
            promotion.Discount.Amount,
            promotion.Discount.Type.ToString(),
            promotion.ValidityPeriod.StartDate,
            promotion.ValidityPeriod.EndDate,
            promotion.RedemptionCap,
            promotion.Status.ToString(),
            promotion.CreatedAt,
            promotion.UpdatedAt,
            promotion.IsActive()
        );
    }

    /// <summary>
    /// Convierte una coleccion de promociones en recursos de respuesta.
    /// </summary>
    /// <param name="promotions">Coleccion de promociones que sera expuesta.</param>
    /// <returns>Coleccion de recursos de respuesta.</returns>
    public static IReadOnlyList<PromotionResource> ToResources(IEnumerable<Promotion> promotions)
    {
        ArgumentNullException.ThrowIfNull(promotions);

        return promotions
            .Select(ToResource)
            .ToList();
    }
}
