namespace Klippr_Backend.Redemption.Interface.Transform;

/// <summary>
/// Representa un canje en las respuestas HTTP.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="Id">Identificador interno del canje.</param>
/// <param name="ConsumerId">Identificador del consumidor que genero el canje.</param>
/// <param name="BusinessId">Identificador del negocio afiliado.</param>
/// <param name="PromotionId">Identificador de la promocion canjeada.</param>
/// <param name="Code">Codigo unico del canje en formato legible sin guiones.</param>
/// <param name="UniqueToken">Token antifraude asociado al canje.</param>
/// <param name="Status">Estado actual del canje como cadena de texto.</param>
/// <param name="ValidationMethod">Metodo de validacion usado o previsto.</param>
/// <param name="DiscountAppliedAmount">Monto de descuento aplicado al canje.</param>
/// <param name="GeneratedAt">Fecha y hora de generacion del canje.</param>
/// <param name="ExpiresAt">Fecha y hora de vencimiento del canje.</param>
/// <param name="RedeemedAt">Fecha y hora de confirmacion del canje o nulo si aun no fue confirmado.</param>
/// <param name="BlockedAt">Fecha y hora de bloqueo del canje o nulo si no fue bloqueado.</param>
public record RedemptionResource(
    int Id,
    Guid ConsumerId,
    Guid BusinessId,
    string PromotionId,
    string Code,
    Guid UniqueToken,
    string Status,
    string ValidationMethod,
    decimal DiscountAppliedAmount,
    DateTimeOffset GeneratedAt,
    DateTimeOffset ExpiresAt,
    DateTimeOffset? RedeemedAt,
    DateTimeOffset? BlockedAt
);
