using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Domain.ValueObjects;

namespace Klippr_Backend.Promotions.Interface.Transform;

/// <summary>
/// Convierte recursos de creacion en comandos de dominio para promociones.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// La conversion construye los value objects requeridos por el dominio sin aplicar
/// reglas de negocio adicionales en la capa de interfaz.
/// </remarks>
public static class CreatePromotionCommandFromResourceAssembler
{
    /// <summary>
    /// Convierte un recurso de creacion en un comando de dominio.
    /// </summary>
    /// <param name="resource">Recurso HTTP con los datos de la promocion.</param>
    /// <returns>Comando de creacion de promocion.</returns>
    public static CreatePromotionCommand ToCommand(CreatePromotionResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new CreatePromotionCommand(
            resource.BusinessId,
            resource.Title,
            resource.Description,
            new DiscountValue(resource.DiscountAmount, DiscountTypeParser.Parse(resource.DiscountType)),
            new TimeFrame(resource.StartDate, resource.EndDate),
            resource.RedemptionCap
        );
    }
}
