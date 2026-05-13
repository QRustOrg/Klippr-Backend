using Klippr_Backend.Redemption.Domain.ValueObjects;

namespace Klippr_Backend.Redemption.Domain.Commands;

/// <summary>
/// Comando que representa la intencion del negocio de confirmar el uso de un canje.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="RedemptionId">Identificador del canje que se desea confirmar.</param>
/// <param name="BusinessId">Identificador del negocio que confirma el canje.</param>
/// <param name="ValidationMethod">Metodo usado para validar el canje.</param>
/// <param name="ConfirmedAt">Fecha y hora en la que se confirma el uso del canje.</param>
public record ConfirmRedemptionCommand(
    int RedemptionId,
    Guid BusinessId,
    RedemptionValidationMethod ValidationMethod,
    DateTimeOffset ConfirmedAt
);
