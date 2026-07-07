using Klippr_Backend.Analytics.Domain.Commands;
using Klippr_Backend.Analytics.Domain.Services;
using Klippr_Backend.Analytics.Interface.Facade;
using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Domain.Queries;
using Klippr_Backend.Promotions.Domain.Services;
using Klippr_Backend.Promotions.Domain.ValueObjects;
using Klippr_Backend.Redemption.Application.Services;
using Klippr_Backend.Redemption.Domain.Commands;
using Klippr_Backend.Redemption.Domain.Exceptions;
using Klippr_Backend.Redemption.Domain.Repositories;
using Klippr_Backend.Redemption.Domain.ValueObjects;
using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Tests;

public class RedemptionCapRaceConditionTests
{
    [Fact]
    public async Task Confirm_RejectsWhenPromotionSlotCannotBeConsumed()
    {
        var promotionId = Guid.NewGuid();
        var redemption = RedemptionAggregate.Create(new RedeemPromotionCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            promotionId.ToString(),
            DateTimeOffset.UtcNow.AddHours(2),
            10m,
            RedemptionValidationMethod.QrScan));

        var redemptionRepository = new FakeRedemptionRepository();
        redemptionRepository.Redemptions.Add(redemption);

        var promotionCommandService = new FakePromotionCommandService(slotAvailable: false);
        var service = new RedemptionCommandService(
            redemptionRepository,
            new FakePromotionQueryService(),
            promotionCommandService,
            new AnalyticsContextFacade(new FakeAnalyticsCommandService()));

        var exception = await Assert.ThrowsAsync<RedemptionConflictException>(() =>
            service.Handle(new ConfirmRedemptionCommand(
                redemption.Id,
                redemption.BusinessId,
                RedemptionValidationMethod.QrScan,
                DateTimeOffset.UtcNow)));

        Assert.Equal("Esta promoción ya alcanzó el límite de canjes.", exception.Message);
        Assert.Equal(RedemptionStatus.Generated, redemption.Status);
        Assert.Empty(redemptionRepository.UpdatedRedemptions);
    }

    [Fact]
    public async Task Confirm_SucceedsWhenPromotionSlotIsAvailable()
    {
        var promotionId = Guid.NewGuid();
        var redemption = RedemptionAggregate.Create(new RedeemPromotionCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            promotionId.ToString(),
            DateTimeOffset.UtcNow.AddHours(2),
            10m,
            RedemptionValidationMethod.QrScan));

        var redemptionRepository = new FakeRedemptionRepository();
        redemptionRepository.Redemptions.Add(redemption);

        var promotionCommandService = new FakePromotionCommandService(slotAvailable: true);
        var service = new RedemptionCommandService(
            redemptionRepository,
            new FakePromotionQueryService(),
            promotionCommandService,
            new AnalyticsContextFacade(new FakeAnalyticsCommandService()));

        var result = await service.Handle(new ConfirmRedemptionCommand(
            redemption.Id,
            redemption.BusinessId,
            RedemptionValidationMethod.QrScan,
            DateTimeOffset.UtcNow));

        Assert.NotNull(result);
        Assert.Equal(RedemptionStatus.Blocked, result!.Status);
        Assert.Equal(1, promotionCommandService.CallCount);
    }

    private sealed class FakeRedemptionRepository : IRedemptionRepository
    {
        public List<RedemptionAggregate> Redemptions { get; } = [];
        public List<RedemptionAggregate> UpdatedRedemptions { get; } = [];

        public Task<RedemptionAggregate?> FindByIdAsync(int id) =>
            Task.FromResult(Redemptions.FirstOrDefault(redemption => redemption.Id == id));

        public Task<RedemptionAggregate?> FindByUniqueTokenAsync(Guid uniqueToken) =>
            Task.FromResult(Redemptions.FirstOrDefault(redemption => redemption.UniqueToken == uniqueToken));

        public Task<IReadOnlyList<RedemptionAggregate>> FindByConsumerAndPromotionAsync(Guid consumerId, string promotionId) =>
            Task.FromResult<IReadOnlyList<RedemptionAggregate>>([]);

        public Task<int> CountFinalizedByPromotionIdAsync(string promotionId) =>
            Task.FromResult(0);

        public Task<IEnumerable<RedemptionAggregate>> FindByConsumerIdAsync(Guid consumerId) =>
            Task.FromResult<IEnumerable<RedemptionAggregate>>([]);

        public Task<IEnumerable<RedemptionAggregate>> FindByBusinessIdAsync(Guid businessId) =>
            Task.FromResult<IEnumerable<RedemptionAggregate>>([]);

        public Task AddAsync(RedemptionAggregate redemption)
        {
            Redemptions.Add(redemption);
            return Task.CompletedTask;
        }

        public void Update(RedemptionAggregate redemption) => UpdatedRedemptions.Add(redemption);

        public void Remove(RedemptionAggregate redemption) => Redemptions.Remove(redemption);

        public Task SaveChangesAsync() => Task.CompletedTask;
    }

    private sealed class FakePromotionQueryService : IPromotionQueryService
    {
        public Task<Promotion?> GetByIdAsync(GetPromotionByIdQuery query, CancellationToken cancellationToken = default) =>
            Task.FromResult<Promotion?>(null);

        public Task<IReadOnlyList<Promotion>> GetByBusinessIdAsync(
            GetPromotionsByBusinessIdQuery query,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Promotion>>([]);

        public Task<IReadOnlyList<Promotion>> GetActiveAsync(
            GetActivePromotionsQuery query,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Promotion>>([]);

        public Task<IReadOnlyList<Promotion>> GetAllAsync(
            GetAllPromotionsQuery query,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Promotion>>([]);
    }

    private sealed class FakePromotionCommandService(bool slotAvailable) : IPromotionCommandService
    {
        public int CallCount { get; private set; }

        public Task<Guid> CreateAsync(CreatePromotionCommand command, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public Task UpdateAsync(UpdatePromotionCommand command, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public Task PublishAsync(PublishPromotionCommand command, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public Task CancelAsync(CancelPromotionCommand command, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public Task DeleteAsync(DeletePromotionCommand command, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public Task<bool> TryConsumeRedemptionSlotAsync(Guid promotionId, CancellationToken cancellationToken = default)
        {
            CallCount++;
            return Task.FromResult(slotAvailable);
        }
    }

    private sealed class FakeAnalyticsCommandService : IAnalyticsCommandService
    {
        public Task Handle(UpdateMetricsCommand command) => Task.CompletedTask;
        public Task Handle(RegisterViewCommand command) => Task.CompletedTask;
        public Task Handle(RegisterAbuseReportCommand command) => Task.CompletedTask;
        public Task Handle(UpdateAbuseReportStatusCommand command) => Task.CompletedTask;
    }
}
