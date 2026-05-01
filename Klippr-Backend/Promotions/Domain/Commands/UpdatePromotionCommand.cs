namespace Klippr_Backend.Promotions.Domain.Commands;

/// <summary>
/// Comando para actualizar los datos editables de una promoción existente.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="PromotionId">Identificador de la promoción que se desea actualizar.</param>
/// <param name="Title">Nuevo título comercial de la promoción.</param>
/// <param name="Description">Nueva descripción visible de la promoción.</param>
/// <param name="Discount">Nuevo valor de descuento ofrecido.</param>
/// <param name="ValidityPeriod">Nuevo periodo de vigencia de la promoción.</param>
/// <param name="RedemptionCap">Nuevo límite máximo de redenciones; <see langword="null"/> indica redenciones ilimitadas.</param>
/// <remarks>
/// Este comando no define restricciones sobre el estado de la promoción. 
/// </remarks>
public record UpdatePromotionCommand(
    Guid PromotionId,
    string Title,
    string Description,
    DiscountValue Discount,
    TimeFrame ValidityPeriod,
    int? RedemptionCap
);
