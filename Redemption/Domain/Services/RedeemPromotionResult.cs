using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Redemption.Domain.Services;

/// <summary>
/// Result of requesting a redemption, including whether a new record was created.
/// </summary>
/// <author>Samuel Bonifacio</author>
public record RedeemPromotionResult(RedemptionAggregate Redemption, bool Created);
