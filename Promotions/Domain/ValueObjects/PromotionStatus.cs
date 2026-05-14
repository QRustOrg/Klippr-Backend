namespace Klippr_Backend.Promotions.Domain.ValueObjects;

/// <summary>
/// Representa los estados del ciclo de vida de una promoción.
/// </summary>
/// <author>Samuel Bonifacio</author>
public enum PromotionStatus
{
    /// <summary>
    /// La promoción fue creada, pero todavía no está disponible para consumidores.
    /// </summary>
    Draft,

    /// <summary>
    /// La promoción está publicada y puede estar disponible durante su vigencia.
    /// </summary>
    Published,

    /// <summary>
    /// La promoción terminó por vencimiento de su periodo de vigencia.
    /// </summary>
    Expired,

    /// <summary>
    /// La promoción fue cancelada antes de completar su ciclo normal.
    /// </summary>
    Cancelled
}
