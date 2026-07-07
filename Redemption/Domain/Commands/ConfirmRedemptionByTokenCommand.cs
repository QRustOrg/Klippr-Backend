using Klippr_Backend.Redemption.Domain.ValueObjects;

namespace Klippr_Backend.Redemption.Domain.Commands;

/// <summary>
/// Command used by a business to confirm a redemption from the opaque QR token.
/// </summary>
/// <author>Samuel Bonifacio</author>
public record ConfirmRedemptionByTokenCommand(
    Guid UniqueToken,
    Guid BusinessId,
    RedemptionValidationMethod ValidationMethod,
    DateTimeOffset ConfirmedAt
);
