namespace Klippr_Backend.Analytics.Interface.Resources;

public record BusinessDashboardResource(
    Guid BusinessId,
    int TotalViews,
    int TotalRedemptions,
    float AverageRating,
    float ConversionRate,
    int TotalCampaigns
);