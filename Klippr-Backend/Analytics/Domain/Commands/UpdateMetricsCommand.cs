namespace Klippr_Backend.Analytics.Domain.Commands;

public record UpdateMetricsCommand(
    Guid CampaignId,
    Guid BusinessId, 
    int? ViewsToAdd,
    int? RedemptionsToAdd,
    float? NewRating
);