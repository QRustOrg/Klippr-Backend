namespace Klippr_Backend.Promotions.Interface.Transform;

/// <summary>
/// Representa los datos de entrada para crear una promocion.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="BusinessId">Identificador del negocio propietario de la promocion.</param>
/// <param name="Title">Titulo comercial de la promocion.</param>
/// <param name="Description">Descripcion visible para consumidores.</param>
/// <param name="DiscountAmount">Monto numerico del descuento.</param>
/// <param name="DiscountType">Tipo de descuento solicitado.</param>
/// <param name="StartDate">Fecha y hora UTC de inicio de vigencia.</param>
/// <param name="EndDate">Fecha y hora UTC de fin de vigencia.</param>
/// <param name="RedemptionCap">Limite maximo de redenciones permitidas.</param>
public record CreatePromotionResource(
    Guid BusinessId,
    string Title,
    string Description,
    decimal DiscountAmount,
    string DiscountType,
    DateTime StartDate,
    DateTime EndDate,
    int? RedemptionCap
);
