using Klippr_Backend.Analytics.Domain.Aggregates;
using Klippr_Backend.Analytics.Domain.Queries;

namespace Klippr_Backend.Analytics.Domain.Services;

public interface IAnalyticsQueryService
{
    Task<CampaignMetrics?> Handle(GetCampaignMetricsQuery query);
    Task<IEnumerable<AbuseReport>> Handle(GetAbuseReportsQuery query);
    
    Task<IEnumerable<CampaignMetrics>> Handle(GetBusinessDashboardQuery query);
}