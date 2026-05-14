using Klippr_Backend.Promotions.Domain.ValueObjects;

namespace Klippr_Backend.Promotions.Domain.Commands;

/// <summary>
/// Comando para crear una nueva promoción en estado borrador.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="BusinessId">Identificador del negocio propietario.</param>
/// <param name="Title">Título comercial de la promoción.</param>
/// <param name="Description">Descripción visible de la promoción.</param>
/// <param name="Discount">Valor del descuento ofrecido por la promoción.</param>
/// <param name="ValidityPeriod">Rango de fechas durante el cual la promoción puede estar vigente.</param>
/// <param name="RedemptionCap">Límite máximo de redenciones permitidas; <see langword="null"/> indica redenciones ilimitadas.</param>
/// <remarks>
/// Este comando es un portador de datos inmutable. Las reglas de negocio y validaciones, se aplican exclusivamente dentro del agregado <c>Promotion</c>.
/// </remarks>
public record CreatePromotionCommand(
    Guid BusinessId,
    string Title,
    string Description,
    DiscountValue Discount,
    TimeFrame ValidityPeriod,
    int? RedemptionCap
);
