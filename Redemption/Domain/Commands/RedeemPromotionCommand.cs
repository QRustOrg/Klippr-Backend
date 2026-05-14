using Klippr_Backend.Redemption.Domain.ValueObjects;

namespace Klippr_Backend.Redemption.Domain.Commands;

/// <summary>
/// Comando que representa la intencion del consumidor de generar un canje para una promocion.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="ConsumerId">Identificador del consumidor que solicita el canje.</param>
/// <param name="BusinessId">Identificador del negocio afiliado asociado a la promocion.</param>
/// <param name="PromotionId">Identificador de la promocion que se desea canjear.</param>
/// <param name="ExpiresAt">Fecha y hora en la que el canje dejara de ser valido.</param>
/// <param name="DiscountAppliedAmount">Monto de descuento aplicado al canje.</param>
/// <param name="ValidationMethod">Metodo previsto para validar el canje.</param>
public record RedeemPromotionCommand(
    Guid ConsumerId,
    Guid BusinessId,
    string PromotionId,
    DateTimeOffset ExpiresAt,
    decimal DiscountAppliedAmount,
    RedemptionValidationMethod ValidationMethod
);
