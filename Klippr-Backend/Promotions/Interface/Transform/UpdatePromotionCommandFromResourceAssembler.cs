using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Domain.ValueObjects;

namespace Klippr_Backend.Promotions.Interface.Transform;

/// <summary>
/// Convierte recursos de actualizacion en comandos de dominio para promociones.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// La conversion mantiene el identificador de la ruta como fuente de verdad para la promocion actualizada.
/// </remarks>
public static class UpdatePromotionCommandFromResourceAssembler
{
    /// <summary>
    /// Convierte un recurso de actualizacion en un comando de dominio.
    /// </summary>
    /// <param name="promotionId">Identificador de la promocion recibido por ruta.</param>
    /// <param name="resource">Recurso HTTP con los datos actualizados.</param>
    /// <returns>Comando de actualizacion de promocion.</returns>
    public static UpdatePromotionCommand ToCommand(Guid promotionId, UpdatePromotionResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new UpdatePromotionCommand(
            promotionId,
            resource.Title,
            resource.Description,
            new DiscountValue(resource.DiscountAmount, DiscountTypeParser.Parse(resource.DiscountType)),
            new TimeFrame(resource.StartDate, resource.EndDate),
            resource.RedemptionCap
        );
    }
}
