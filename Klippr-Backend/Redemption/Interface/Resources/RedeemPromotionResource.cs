namespace Klippr_Backend.Redemption.Interface.Resources;

/// <summary>
/// Datos de entrada para generar un nuevo canje de promocion.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="ConsumerId">Identificador del consumidor que solicita el canje.</param>
/// <param name="BusinessId">Identificador del negocio afiliado.</param>
/// <param name="PromotionId">Identificador de la promocion a canjear.</param>
/// <param name="ExpiresAt">Fecha y hora en la que el canje dejara de ser valido.</param>
/// <param name="DiscountAppliedAmount">Monto de descuento que se aplicara.</param>
/// <param name="ValidationMethod">Metodo previsto para validar el canje (QrScan o ManualCode).</param>
public record RedeemPromotionResource(
    Guid ConsumerId,
    Guid BusinessId,
    string PromotionId,
    DateTimeOffset ExpiresAt,
    decimal DiscountAppliedAmount,
    string ValidationMethod
);
