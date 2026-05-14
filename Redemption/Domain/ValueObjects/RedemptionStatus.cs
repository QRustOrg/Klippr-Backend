namespace Klippr_Backend.Redemption.Domain.ValueObjects;

/// <summary>
/// Representa los estados permitidos durante el ciclo de vida de un canje.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// El flujo de estado valido es: Generated -> Redeemed -> Blocked, o Generated -> Expired.
/// </remarks>
public enum RedemptionStatus
{
    /// <summary>
    /// El canje fue generado y aun puede ser utilizado si no expiro.
    /// </summary>
    Generated,

    /// <summary>
    /// El canje fue confirmado como usado por el negocio.
    /// </summary>
    Redeemed,

    /// <summary>
    /// El canje fue bloqueado para evitar reutilizacion.
    /// </summary>
    Blocked,

    /// <summary>
    /// El canje vencio antes de ser usado.
    /// </summary>
    Expired
}
