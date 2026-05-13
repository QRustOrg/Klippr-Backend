namespace Klippr_Backend.Promotions.Interface.Resources;

/// <summary>
/// Representa los datos de entrada para actualizar una promocion en borrador.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="Title">Nuevo titulo comercial de la promocion.</param>
/// <param name="Description">Nueva descripcion visible para consumidores.</param>
/// <param name="DiscountAmount">Nuevo monto numerico del descuento.</param>
/// <param name="DiscountType">Nuevo tipo de descuento solicitado.</param>
/// <param name="StartDate">Nueva fecha y hora UTC de inicio de vigencia.</param>
/// <param name="EndDate">Nueva fecha y hora UTC de fin de vigencia.</param>
/// <param name="RedemptionCap">Nuevo limite maximo de redenciones permitidas.</param>
public record UpdatePromotionResource(
    string Title,
    string Description,
    decimal DiscountAmount,
    string DiscountType,
    DateTime StartDate,
    DateTime EndDate,
    int? RedemptionCap
);
