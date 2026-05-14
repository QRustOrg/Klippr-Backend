using Klippr_Backend.Analytics.Domain.Aggregates;
using Klippr_Backend.Analytics.Domain.Queries;
using Klippr_Backend.Analytics.Domain.Repositories;
using Klippr_Backend.Analytics.Domain.Services;
using Klippr_Backend.Analytics.Domain.ValueObjects;

namespace Klippr_Backend.Analytics.Application.Services.QueryServices;

public class AnalyticsQueryService : IAnalyticsQueryService
{
    private readonly ICampaignMetricsRepository _metricsRepository;
    private readonly IAbuseReportRepository _abuseRepository;

    public AnalyticsQueryService(
        ICampaignMetricsRepository metricsRepository,
        IAbuseReportRepository abuseRepository)
    {
        _metricsRepository = metricsRepository;
        _abuseRepository = abuseRepository;
    }

    public async Task<CampaignMetrics?> Handle(GetCampaignMetricsQuery query)
    {
        return await _metricsRepository.FindByCampaignIdAsync(query.CampaignId);
    }

    public async Task<IEnumerable<CampaignMetrics>> Handle(GetBusinessDashboardQuery query)
    {
        return await _metricsRepository.FindByBusinessIdAsync(query.BusinessId);
    }

    public async Task<IEnumerable<AbuseReport>> Handle(GetAbuseReportsQuery query)
    {
        if (query.Status.HasValue)
            return await _abuseRepository.FindByStatusAsync(query.Status.Value);

        return await _abuseRepository.FindAllAsync();
    }
}