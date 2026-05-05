using Klippr_Backend.Analytics.Domain.ValueObjects;

namespace Klippr_Backend.Analytics.Domain.Aggregates;

using Klippr_Backend.Analytics.Domain.ValueObjects;

public class CampaignMetrics
{
    public CampaignMetricsId Id { get; private set; }
    public Guid CampaignId { get; private set; }
    public Guid BusinessId { get; private set; }

    public int Views { get; private set; }
    public int Redemptions { get; private set; }
    public float AverageRating { get; private set; }
    public int RatingCount { get; private set; }
    public DateTime LastUpdated { get; private set; }

    protected CampaignMetrics() { }

    public CampaignMetrics(Guid campaignId, Guid businessId)
    {
        Id = new CampaignMetricsId(Guid.NewGuid());
        CampaignId = campaignId;
        BusinessId = businessId;

        Views = 0;
        Redemptions = 0;
        AverageRating = 0;
        RatingCount = 0;
        LastUpdated = DateTime.UtcNow;
    }

    public void AddViews(int count)
    {
        if (count <= 0) return;

        Views += count;
        Touch();
    }

    public void AddRedemptions(int count)
    {
        if (count <= 0) return;

        Redemptions += count;
        Touch();
    }

    public void UpdateRating(float newRating)
    {
        if (newRating < 1 || newRating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.");

        AverageRating = ((AverageRating * RatingCount) + newRating) / (RatingCount + 1);
        RatingCount++;

        Touch();
    }

    public float CalculateConversionRate()
    {
        if (Views == 0) return 0;
        return (float)Redemptions / Views;
    }

    private void Touch()
    {
        LastUpdated = DateTime.UtcNow;
    }
}