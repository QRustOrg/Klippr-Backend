using Klippr_Backend.Analytics.Domain.Commands;

namespace Klippr_Backend.Analytics.Domain.Services;


public interface IAnalyticsCommandService
{
    Task Handle(UpdateMetricsCommand command);
    Task Handle(RegisterAbuseReportCommand command);
}