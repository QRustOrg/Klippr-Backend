using Klippr_Backend.Analytics.Domain.Aggregates;

namespace Klippr_Backend.Analytics.Domain.Repositories;


public interface ICampaignMetricsRepository
{
    Task<CampaignMetrics?> FindByCampaignIdAsync(Guid campaignId);
    
    Task<IEnumerable<CampaignMetrics>> FindByBusinessIdAsync(Guid businessId);
    Task AddAsync(CampaignMetrics metrics);
    Task UpdateAsync(CampaignMetrics metrics);
    
}