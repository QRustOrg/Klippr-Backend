namespace Klippr_Backend.Analytics.Interface.Resources;

public record UpdateMetricsResource(
    Guid CampaignId,
    Guid BusinessId,
    int? ViewsToAdd,
    int? RedemptionsToAdd,
    float? NewRating
);