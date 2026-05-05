using Klippr_Backend.Analytics.Domain.Aggregates;
using Klippr_Backend.Analytics.Interface.Resources;

namespace Klippr_Backend.Analytics.Interface.Assemblers;

public static class BusinessDashboardResourceFromEntityAssembler
{
    public static BusinessDashboardResource ToResource(
        Guid businessId,
        IEnumerable<CampaignMetrics> metrics)
    {
        var list = metrics.ToList();

        var totalViews = list.Sum(m => m.Views);
        var totalRedemptions = list.Sum(m => m.Redemptions);

        var averageRating = list.Any()
            ? list.Average(m => m.AverageRating)
            : 0;

        var conversionRate = totalViews == 0
            ? 0
            : (float)totalRedemptions / totalViews;

        return new BusinessDashboardResource(
            businessId,
            totalViews,
            totalRedemptions,
            averageRating,
            conversionRate,
            list.Count
        );
    }
}