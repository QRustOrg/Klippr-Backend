using Klippr_Backend.Analytics.Application.Services.CommandServices;
using Klippr_Backend.Analytics.Domain.Aggregates;
using Klippr_Backend.Analytics.Domain.Commands;
using Klippr_Backend.Analytics.Domain.Repositories;
using Klippr_Backend.Analytics.Domain.ValueObjects;

namespace Klippr_Backend.Tests;

public class RegisterViewTests
{
    [Fact]
    public async Task Handle_AddsExactlyOneViewRegardlessOfRepeatedCalls()
    {
        var campaignId = Guid.NewGuid();
        var businessId = Guid.NewGuid();
        var repository = new FakeCampaignMetricsRepository();
        var service = new AnalyticsCommandService(repository, new FakeAbuseReportRepository());

        await service.Handle(new RegisterViewCommand(campaignId, businessId));
        await service.Handle(new RegisterViewCommand(campaignId, businessId));
        await service.Handle(new RegisterViewCommand(campaignId, businessId));

        var metrics = await repository.FindByCampaignIdAsync(campaignId);
        Assert.NotNull(metrics);
        Assert.Equal(3, metrics!.Views);
    }

    [Fact]
    public async Task Handle_CreatesMetricsForNewCampaignWithSingleView()
    {
        var campaignId = Guid.NewGuid();
        var businessId = Guid.NewGuid();
        var repository = new FakeCampaignMetricsRepository();
        var service = new AnalyticsCommandService(repository, new FakeAbuseReportRepository());

        await service.Handle(new RegisterViewCommand(campaignId, businessId));

        var metrics = await repository.FindByCampaignIdAsync(campaignId);
        Assert.NotNull(metrics);
        Assert.Equal(1, metrics!.Views);
        Assert.Equal(0, metrics.Redemptions);
        Assert.Equal(businessId, metrics.BusinessId);
    }

    private sealed class FakeCampaignMetricsRepository : ICampaignMetricsRepository
    {
        private readonly Dictionary<Guid, CampaignMetrics> _metricsByCampaignId = [];

        public Task<CampaignMetrics?> FindByCampaignIdAsync(Guid campaignId) =>
            Task.FromResult(_metricsByCampaignId.GetValueOrDefault(campaignId));

        public Task<IEnumerable<CampaignMetrics>> FindByBusinessIdAsync(Guid businessId) =>
            Task.FromResult<IEnumerable<CampaignMetrics>>(_metricsByCampaignId.Values.Where(m => m.BusinessId == businessId));

        public Task AddAsync(CampaignMetrics metrics)
        {
            _metricsByCampaignId[metrics.CampaignId] = metrics;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(CampaignMetrics metrics)
        {
            _metricsByCampaignId[metrics.CampaignId] = metrics;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeAbuseReportRepository : IAbuseReportRepository
    {
        public Task AddAsync(AbuseReport report) => Task.CompletedTask;
        public Task<IEnumerable<AbuseReport>> FindAllAsync() => Task.FromResult<IEnumerable<AbuseReport>>([]);
        public Task<IEnumerable<AbuseReport>> FindByStatusAsync(AbuseReportStatus status) => Task.FromResult<IEnumerable<AbuseReport>>([]);
        public Task<AbuseReport?> FindByIdAsync(Guid reportId) => Task.FromResult<AbuseReport?>(null);
        public Task UpdateAsync(AbuseReport report) => Task.CompletedTask;
    }
}
