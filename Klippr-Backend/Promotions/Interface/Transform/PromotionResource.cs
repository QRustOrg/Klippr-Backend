namespace Klippr_Backend.Promotions.Interface.Transform;

/// <summary>
/// Representa una promocion en las respuestas HTTP.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="Id">Identificador unico de la promocion.</param>
/// <param name="BusinessId">Identificador del negocio propietario.</param>
/// <param name="Title">Titulo comercial de la promocion.</param>
/// <param name="Description">Descripcion visible de la promocion.</param>
/// <param name="DiscountAmount">Monto numerico del descuento.</param>
/// <param name="DiscountType">Tipo de descuento aplicado.</param>
/// <param name="StartDate">Fecha y hora UTC de inicio de vigencia.</param>
/// <param name="EndDate">Fecha y hora UTC de fin de vigencia.</param>
/// <param name="RedemptionCap">Limite maximo de redenciones permitidas.</param>
/// <param name="Status">Estado actual de la promocion.</param>
/// <param name="CreatedAt">Fecha y hora UTC de creacion.</param>
/// <param name="UpdatedAt">Fecha y hora UTC de ultima actualizacion.</param>
/// <param name="IsActive">Indica si la promocion esta publicada y vigente.</param>
public record PromotionResource(
    Guid Id,
    Guid BusinessId,
    string Title,
    string Description,
    decimal DiscountAmount,
    string DiscountType,
    DateTime StartDate,
    DateTime EndDate,
    int? RedemptionCap,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsActive
);
