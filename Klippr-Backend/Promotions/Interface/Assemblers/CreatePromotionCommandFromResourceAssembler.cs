using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Domain.ValueObjects;
using Klippr_Backend.Promotions.Interface.Resources;

namespace Klippr_Backend.Promotions.Interface.Assemblers;

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
            new DiscountValue(resource.DiscountAmount, ParseDiscountType(resource.DiscountType)),
            new TimeFrame(resource.StartDate, resource.EndDate),
            resource.RedemptionCap
        );
    }

    private static DiscountType ParseDiscountType(string discountType)
    {
        if (Enum.TryParse<DiscountType>(discountType, true, out var parsedDiscountType))
            return parsedDiscountType;

        throw new ArgumentException("Discount type is invalid.", nameof(discountType));
    }
}
