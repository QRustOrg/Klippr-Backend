using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;
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
    /// <returns>Canje generado o reutilizado, o <see langword="null"/> si no pudo completarse.</returns>
    Task<RedeemPromotionResult?> Handle(RedeemPromotionCommand command);

    /// <summary>
    /// Confirma el uso de un canje existente.
    /// </summary>
    /// <param name="command">Datos necesarios para confirmar el canje.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Canje confirmado o <see langword="null"/> si no pudo completarse.</returns>
    Task<RedemptionAggregate?> Handle(ConfirmRedemptionCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirma el uso de un canje existente a partir de su token unico.
    /// </summary>
    /// <param name="command">Datos necesarios para confirmar el canje por token.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Canje confirmado o <see langword="null"/> si no pudo completarse.</returns>
    Task<RedemptionAggregate?> Handle(ConfirmRedemptionByTokenCommand command, CancellationToken cancellationToken = default);
}
