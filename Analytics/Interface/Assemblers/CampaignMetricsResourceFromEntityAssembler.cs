using Klippr_Backend.Analytics.Domain.Aggregates;
using Klippr_Backend.Analytics.Interface.Resources;

namespace Klippr_Backend.Analytics.Interface.Assemblers;

public static class CampaignMetricsResourceFromEntityAssembler
{
    public static CampaignMetricsResource ToResourceFromEntity(CampaignMetrics entity)
    {
        return new CampaignMetricsResource(
            entity.Id.Value,
            entity.CampaignId,
            entity.BusinessId,
            entity.Views,
            entity.Redemptions,
            entity.AverageRating,
            entity.CalculateConversionRate(),
            entity.LastUpdated
        );
    }

    public static IEnumerable<CampaignMetricsResource> ToResourceList(IEnumerable<CampaignMetrics> entities)
    {
        return entities.Select(ToResourceFromEntity);
    }
}