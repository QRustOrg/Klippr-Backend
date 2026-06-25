using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Domain.Queries;
using Klippr_Backend.Promotions.Domain.Services;
using Klippr_Backend.Promotions.Domain.ValueObjects;
using Klippr_Backend.Analytics.Domain.Commands;
using Klippr_Backend.Analytics.Domain.Services;
using Klippr_Backend.Analytics.Interface.Facade;
using Klippr_Backend.Redemption.Application.Services;
using Klippr_Backend.Redemption.Domain.Commands;
using Klippr_Backend.Redemption.Domain.Exceptions;
using Klippr_Backend.Redemption.Domain.Repositories;
using Klippr_Backend.Redemption.Domain.ValueObjects;
using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Tests;

public class RedemptionReusePolicyTests
{
    [Fact]
    public async Task Redeem_ReusesExistingGeneratedRedemptionForSameConsumerAndPromotion()
    {
        var promotionId = Guid.NewGuid();
        var repository = new FakeRedemptionRepository();
        repository.Redemptions.Add(CreateRedemption(promotionId, Guid.NewGuid(), expiresIn: TimeSpan.FromHours(1)));
        var service = CreateService(repository, promotionId);

        var result = await service.Handle(CreateCommand(promotionId, repository.Redemptions[0].ConsumerId));

        Assert.NotNull(result);
        Assert.False(result!.Created);
        Assert.Equal(repository.Redemptions[0].UniqueToken, result.Redemption.UniqueToken);
        Assert.Empty(repository.AddedRedemptions);
    }

    [Fact]
    public async Task Redeem_BlocksNewAttemptWhenPromotionWasAlreadyRedeemedByConsumer()
    {
        var promotionId = Guid.NewGuid();
        var consumerId = Guid.NewGuid();
        var repository = new FakeRedemptionRepository();
        var usedRedemption = CreateRedemption(promotionId, consumerId, expiresIn: TimeSpan.FromHours(1));
        usedRedemption.Confirm(DateTimeOffset.UtcNow, RedemptionValidationMethod.QrScan);
        repository.Redemptions.Add(usedRedemption);
        var service = CreateService(repository, promotionId);

        var exception = await Assert.ThrowsAsync<RedemptionConflictException>(() =>
            service.Handle(CreateCommand(promotionId, consumerId)));

        Assert.Equal("Ya canjeaste esta promoción.", exception.Message);
    }

    [Fact]
    public async Task Redeem_CreatesNewAttemptAfterPreviousGeneratedRedemptionExpired()
    {
        var promotionId = Guid.NewGuid();
        var consumerId = Guid.NewGuid();
        var repository = new FakeRedemptionRepository();
        var expiredRedemption = CreateRedemption(promotionId, consumerId, expiresIn: TimeSpan.FromMilliseconds(1));
        repository.Redemptions.Add(expiredRedemption);
        await Task.Delay(20);
        var service = CreateService(repository, promotionId);

        var result = await service.Handle(CreateCommand(promotionId, consumerId));

        Assert.NotNull(result);
        Assert.True(result!.Created);
        Assert.Equal(RedemptionStatus.Expired, expiredRedemption.Status);
        Assert.Single(repository.AddedRedemptions);
        Assert.NotEqual(expiredRedemption.UniqueToken, result.Redemption.UniqueToken);
    }

    [Fact]
    public async Task ConfirmByToken_BlocksConfirmedRedemption()
    {
        var promotionId = Guid.NewGuid();
        var repository = new FakeRedemptionRepository();
        var redemption = CreateRedemption(promotionId, Guid.NewGuid(), expiresIn: TimeSpan.FromHours(1));
        repository.Redemptions.Add(redemption);
        var service = CreateService(repository, promotionId);

        var result = await service.Handle(new ConfirmRedemptionByTokenCommand(
            redemption.UniqueToken,
            redemption.BusinessId,
            RedemptionValidationMethod.QrScan,
            DateTimeOffset.UtcNow));

        Assert.NotNull(result);
        Assert.Equal(RedemptionStatus.Blocked, result!.Status);
        Assert.NotNull(result.RedeemedAt);
        Assert.NotNull(result.BlockedAt);
    }

    [Fact]
    public async Task Redeem_RejectsWhenPromotionCapIsReachedByFinalizedRedemptions()
    {
        var promotionId = Guid.NewGuid();
        var repository = new FakeRedemptionRepository();
        var finalized = CreateRedemption(promotionId, Guid.NewGuid(), expiresIn: TimeSpan.FromHours(1));
        finalized.Confirm(DateTimeOffset.UtcNow, RedemptionValidationMethod.QrScan);
        finalized.Block(DateTimeOffset.UtcNow);
        repository.Redemptions.Add(finalized);
        var service = CreateService(repository, promotionId, redemptionCap: 1);

        var exception = await Assert.ThrowsAsync<RedemptionConflictException>(() =>
            service.Handle(CreateCommand(promotionId, Guid.NewGuid())));

        Assert.Equal("Esta promoción ya alcanzó el límite de canjes.", exception.Message);
    }

    private static RedemptionCommandService CreateService(
        FakeRedemptionRepository repository,
        Guid promotionId,
        int? redemptionCap = null)
    {
        var promotionService = new FakePromotionQueryService();
        promotionService.Promotions[promotionId] = CreatePromotion(promotionId, redemptionCap);
        return new RedemptionCommandService(repository, promotionService, new AnalyticsContextFacade(new FakeAnalyticsCommandService()));
    }

    private static RedeemPromotionCommand CreateCommand(Guid promotionId, Guid consumerId) => new(
        consumerId,
        Guid.NewGuid(),
        promotionId.ToString(),
        DateTimeOffset.UtcNow.AddHours(2),
        10m,
        RedemptionValidationMethod.QrScan);

    private static RedemptionAggregate CreateRedemption(Guid promotionId, Guid consumerId, TimeSpan expiresIn) =>
        RedemptionAggregate.Create(new RedeemPromotionCommand(
            consumerId,
            Guid.NewGuid(),
            promotionId.ToString(),
            DateTimeOffset.UtcNow.Add(expiresIn),
            10m,
            RedemptionValidationMethod.QrScan));

    private static Promotion CreatePromotion(Guid promotionId, int? redemptionCap)
    {
        var promotion = Promotion.Create(new CreatePromotionCommand(
            Guid.NewGuid(),
            "Pizza Hot",
            "Promo",
            new DiscountValue(10m, DiscountType.Percentage),
            new TimeFrame(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1)),
            redemptionCap,
            null));

        typeof(Promotion)
            .GetProperty(nameof(Promotion.Id))!
            .SetValue(promotion, promotionId);

        return promotion;
    }

    private sealed class FakeRedemptionRepository : IRedemptionRepository
    {
        public List<RedemptionAggregate> Redemptions { get; } = [];
        public List<RedemptionAggregate> AddedRedemptions { get; } = [];

        public Task<RedemptionAggregate?> FindByIdAsync(int id) =>
            Task.FromResult(Redemptions.FirstOrDefault(redemption => redemption.Id == id));

        public Task<RedemptionAggregate?> FindByUniqueTokenAsync(Guid uniqueToken) =>
            Task.FromResult(Redemptions.FirstOrDefault(redemption => redemption.UniqueToken == uniqueToken));

        public Task<IReadOnlyList<RedemptionAggregate>> FindByConsumerAndPromotionAsync(Guid consumerId, string promotionId) =>
            Task.FromResult<IReadOnlyList<RedemptionAggregate>>(Redemptions
                .Where(redemption => redemption.ConsumerId == consumerId && redemption.PromotionId == promotionId)
                .OrderByDescending(redemption => redemption.GeneratedAt)
                .ToList());

        public Task<int> CountFinalizedByPromotionIdAsync(string promotionId) =>
            Task.FromResult(Redemptions.Count(redemption =>
                redemption.PromotionId == promotionId &&
                redemption.Status is RedemptionStatus.Redeemed or RedemptionStatus.Blocked));

        public Task<IEnumerable<RedemptionAggregate>> FindByConsumerIdAsync(Guid consumerId) =>
            Task.FromResult<IEnumerable<RedemptionAggregate>>(Redemptions.Where(redemption => redemption.ConsumerId == consumerId));

        public Task<IEnumerable<RedemptionAggregate>> FindByBusinessIdAsync(Guid businessId) =>
            Task.FromResult<IEnumerable<RedemptionAggregate>>(Redemptions.Where(redemption => redemption.BusinessId == businessId));

        public Task AddAsync(RedemptionAggregate redemption)
        {
            Redemptions.Add(redemption);
            AddedRedemptions.Add(redemption);
            return Task.CompletedTask;
        }

        public void Update(RedemptionAggregate redemption) { }

        public void Remove(RedemptionAggregate redemption) => Redemptions.Remove(redemption);

        public Task SaveChangesAsync() => Task.CompletedTask;
    }

    private sealed class FakePromotionQueryService : IPromotionQueryService
    {
        public Dictionary<Guid, Promotion> Promotions { get; } = [];

        public Task<Promotion?> GetByIdAsync(GetPromotionByIdQuery query, CancellationToken cancellationToken = default) =>
            Task.FromResult(Promotions.GetValueOrDefault(query.PromotionId));

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

    private sealed class FakeAnalyticsCommandService : IAnalyticsCommandService
    {
        public Task Handle(UpdateMetricsCommand command) => Task.CompletedTask;
        public Task Handle(RegisterAbuseReportCommand command) => Task.CompletedTask;
        public Task Handle(UpdateAbuseReportStatusCommand command) => Task.CompletedTask;
    }
}
