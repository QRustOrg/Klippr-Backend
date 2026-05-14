using Klippr_Backend.Analytics.Domain.Aggregates;
using Klippr_Backend.Analytics.Domain.Commands;
using Klippr_Backend.Analytics.Domain.Repositories;
using Klippr_Backend.Analytics.Domain.Services;

namespace Klippr_Backend.Analytics.Application.Services.CommandServices;

public class AnalyticsCommandService : IAnalyticsCommandService
{
    private readonly ICampaignMetricsRepository _metricsRepository;
    private readonly IAbuseReportRepository _abuseRepository;

    public AnalyticsCommandService(
        ICampaignMetricsRepository metricsRepository,
        IAbuseReportRepository abuseRepository)
    {
        _metricsRepository = metricsRepository;
        _abuseRepository = abuseRepository;
    }

    public async Task Handle(UpdateMetricsCommand command)
    {
        var metrics = await _metricsRepository.FindByCampaignIdAsync(command.CampaignId);

        if (metrics == null)
        {
            metrics = new CampaignMetrics(command.CampaignId, command.BusinessId);

            if (command.ViewsToAdd.HasValue)
                metrics.AddViews(command.ViewsToAdd.Value);

            if (command.RedemptionsToAdd.HasValue)
                metrics.AddRedemptions(command.RedemptionsToAdd.Value);

            if (command.NewRating.HasValue)
                metrics.UpdateRating(command.NewRating.Value);

            await _metricsRepository.AddAsync(metrics);
            return;
        }

        if (command.ViewsToAdd.HasValue)
            metrics.AddViews(command.ViewsToAdd.Value);

        if (command.RedemptionsToAdd.HasValue)
            metrics.AddRedemptions(command.RedemptionsToAdd.Value);

        if (command.NewRating.HasValue)
            metrics.UpdateRating(command.NewRating.Value);

        await _metricsRepository.UpdateAsync(metrics);
    }

    public async Task Handle(RegisterAbuseReportCommand command)
    {
        var report = new AbuseReport(
            command.ReportedEntityId,
            command.ReportedBy,
            command.Reason,
            command.Description
        );

        await _abuseRepository.AddAsync(report);
    }
}