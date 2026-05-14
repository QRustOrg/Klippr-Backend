namespace Klippr_Backend.Redemption.Domain.ValueObjects;

/// <summary>
/// Representa el mecanismo utilizado para validar un canje.
/// </summary>
/// <author>Samuel Bonifacio</author>
public enum RedemptionValidationMethod
{
    /// <summary>
    /// El canje fue validado mediante escaneo de codigo QR.
    /// </summary>
    QrScan,

    /// <summary>
    /// El canje fue validado mediante ingreso manual del codigo.
    /// </summary>
    ManualCode
}
