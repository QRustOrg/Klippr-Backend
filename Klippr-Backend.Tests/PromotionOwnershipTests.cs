using Klippr_Backend.Promotions.Application.Services;
using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Domain.Exceptions;
using Klippr_Backend.Promotions.Domain.Repositories;
using Klippr_Backend.Promotions.Domain.ValueObjects;

namespace Klippr_Backend.Tests;

public class PromotionOwnershipTests
{
    [Fact]
    public async Task UpdateAsync_RejectsRequestFromNonOwnerBusiness()
    {
        var (repository, promotion) = CreateRepositoryWithPromotion();
        var service = new PromotionCommandService(repository);

        var command = new UpdatePromotionCommand(
            promotion.Id,
            Guid.NewGuid(),
            "Nuevo titulo",
            "Nueva descripcion",
            new DiscountValue(20, DiscountType.Percentage),
            new TimeFrame(DateTime.UtcNow, DateTime.UtcNow.AddDays(5)),
            null,
            null);

        await Assert.ThrowsAsync<UnauthorizedPromotionAccessException>(() => service.UpdateAsync(command));
    }

    [Fact]
    public async Task CancelAsync_RejectsRequestFromNonOwnerBusiness()
    {
        var (repository, promotion) = CreateRepositoryWithPromotion();
        var service = new PromotionCommandService(repository);

        await Assert.ThrowsAsync<UnauthorizedPromotionAccessException>(() =>
            service.CancelAsync(new CancelPromotionCommand(promotion.Id, Guid.NewGuid())));
    }

    [Fact]
    public async Task DeleteAsync_RejectsRequestFromNonOwnerBusiness()
    {
        var (repository, promotion) = CreateRepositoryWithPromotion();
        var service = new PromotionCommandService(repository);

        await Assert.ThrowsAsync<UnauthorizedPromotionAccessException>(() =>
            service.DeleteAsync(new DeletePromotionCommand(promotion.Id, Guid.NewGuid())));
    }

    [Fact]
    public async Task CancelAsync_SucceedsWhenRequestingBusinessOwnsPromotion()
    {
        var (repository, promotion) = CreateRepositoryWithPromotion();
        var service = new PromotionCommandService(repository);

        await service.CancelAsync(new CancelPromotionCommand(promotion.Id, promotion.BusinessId));

        Assert.Equal(PromotionStatus.Cancelled, promotion.Status);
    }

    private static (FakePromotionRepository repository, Promotion promotion) CreateRepositoryWithPromotion()
    {
        var businessId = Guid.NewGuid();
        var promotion = Promotion.Create(new CreatePromotionCommand(
            businessId,
            "Pizza Hot",
            "Promo",
            new DiscountValue(10m, DiscountType.Percentage),
            new TimeFrame(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1)),
            null,
            null));

        var repository = new FakePromotionRepository();
        repository.Promotions[promotion.Id] = promotion;
        return (repository, promotion);
    }

    private sealed class FakePromotionRepository : IPromotionRepository
    {
        public Dictionary<Guid, Promotion> Promotions { get; } = [];

        public Task<Promotion?> GetByIdAsync(Guid promotionId, CancellationToken cancellationToken = default) =>
            Task.FromResult(Promotions.GetValueOrDefault(promotionId));

        public Task<IReadOnlyList<Promotion>> GetByBusinessIdAsync(Guid businessId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Promotion>>(Promotions.Values.Where(p => p.BusinessId == businessId).ToList());

        public Task<IReadOnlyList<Promotion>> GetActiveAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Promotion>>([]);

        public Task<IReadOnlyList<Promotion>> GetAllAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Promotion>>(Promotions.Values.ToList());

        public Task AddAsync(Promotion promotion, CancellationToken cancellationToken = default)
        {
            Promotions[promotion.Id] = promotion;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Promotion promotion, CancellationToken cancellationToken = default)
        {
            Promotions[promotion.Id] = promotion;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Promotion promotion, CancellationToken cancellationToken = default)
        {
            Promotions.Remove(promotion.Id);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}
