namespace Klippr_Backend.Analytics.Domain.Queries;

public class GetCampaignMetricsQuery
{
    public Guid CampaignId { get; set; }

    public GetCampaignMetricsQuery(Guid campaignId)
    {
        CampaignId = campaignId;
    }
}