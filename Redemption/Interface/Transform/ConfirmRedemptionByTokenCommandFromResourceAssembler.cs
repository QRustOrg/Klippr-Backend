using Klippr_Backend.Redemption.Domain.Commands;
using Klippr_Backend.Redemption.Domain.ValueObjects;

namespace Klippr_Backend.Redemption.Interface.Transform;

/// <summary>
/// Assembler for confirming a redemption from the opaque QR token.
/// </summary>
/// <author>Samuel Bonifacio</author>
public static class ConfirmRedemptionByTokenCommandFromResourceAssembler
{
    /// <summary>
    /// Converts a REST resource into a token-based confirmation command.
    /// </summary>
    public static ConfirmRedemptionByTokenCommand ToCommand(Guid uniqueToken, ConfirmRedemptionResource resource)
    {
        if (!Enum.TryParse<RedemptionValidationMethod>(resource.ValidationMethod, ignoreCase: true, out var validationMethod))
            throw new ArgumentException(
                $"Metodo de validacion no reconocido: '{resource.ValidationMethod}'. Valores validos: QrScan, ManualCode.",
                nameof(resource.ValidationMethod));

        return new ConfirmRedemptionByTokenCommand(
            uniqueToken,
            resource.BusinessId,
            validationMethod,
            resource.ConfirmedAt);
    }
}
