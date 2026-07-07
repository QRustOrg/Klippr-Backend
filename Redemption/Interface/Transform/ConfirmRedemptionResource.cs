namespace Klippr_Backend.Redemption.Interface.Transform;

/// <summary>
/// Datos de entrada para confirmar el uso de un canje existente.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="BusinessId">Identificador del negocio que confirma el canje.</param>
/// <param name="ValidationMethod">Metodo utilizado para validar el canje (QrScan o ManualCode).</param>
/// <param name="ConfirmedAt">Fecha y hora en la que se confirma el uso del canje.</param>
/// <param name="Signature">Firma HMAC del token, requerida para confirmar por QR (<c>tokens/{uniqueToken}/confirm</c>).</param>
public record ConfirmRedemptionResource(
    Guid BusinessId,
    string ValidationMethod,
    DateTimeOffset ConfirmedAt,
    string? Signature = null
);
