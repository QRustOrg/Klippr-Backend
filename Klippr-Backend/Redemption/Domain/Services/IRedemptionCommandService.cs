using Klippr_Backend.Redemption.Domain.Aggregates;
using Klippr_Backend.Redemption.Domain.Commands;

namespace Klippr_Backend.Redemption.Domain.Services;

/// <summary>
/// Define las operaciones de escritura disponibles para el agregado de canjes.
/// </summary>
/// <author>Samuel Bonifacio</author>
public interface IRedemptionCommandService
{
    /// <summary>
    /// Genera un nuevo canje para una promocion.
    /// </summary>
    /// <param name="command">Datos necesarios para generar el canje.</param>
    /// <returns>Canje generado o <see langword="null"/> si no pudo completarse.</returns>
    Task<Redemption?> Handle(RedeemPromotionCommand command);

    /// <summary>
    /// Confirma el uso de un canje existente.
    /// </summary>
    /// <param name="command">Datos necesarios para confirmar el canje.</param>
    /// <returns>Canje confirmado o <see langword="null"/> si no pudo completarse.</returns>
    Task<Redemption?> Handle(ConfirmRedemptionCommand command);
}
