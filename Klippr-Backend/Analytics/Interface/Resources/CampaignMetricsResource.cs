namespace Klippr_Backend.Analytics.Interface.Resources;

public record CampaignMetricsResource(
    Guid Id,
    Guid CampaignId,
    Guid BusinessId, 
    int Views,
    int Redemptions,
    float AverageRating,
    float ConversionRate,
    DateTime LastUpdated
);