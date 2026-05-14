using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Promotions.Domain.Repositories;
using Klippr_Backend.Promotions.Domain.ValueObjects;
using Klippr_Backend.Promotions.Infrastructure.EventPublishing;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Promotions.Infrastructure.Persistence;

/// <summary>
/// Implementa el repositorio de promociones usando EF Core.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// La implementacion se limita a persistencia y despacho de eventos pendientes luego
/// de confirmar los cambios del contexto.
/// </remarks>
public class PromotionRepository(
    PromotionDbContext dbContext,
    PromotionEventPublisher eventPublisher) : IPromotionRepository
{
    /// <inheritdoc />
    public Task<Promotion?> GetByIdAsync(Guid promotionId, CancellationToken cancellationToken = default)
    {
        return dbContext.Promotions
            .FirstOrDefaultAsync(promotion => promotion.Id == promotionId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Promotion>> GetByBusinessIdAsync(
        Guid businessId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Promotions
            .Where(promotion => promotion.BusinessId == businessId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Promotion>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        return await dbContext.Promotions
            .Where(promotion =>
                promotion.Status == PromotionStatus.Published &&
                promotion.ValidityPeriod.StartDate <= now &&
                promotion.ValidityPeriod.EndDate >= now)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddAsync(Promotion promotion, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        await dbContext.Promotions
            .AddAsync(promotion, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task UpdateAsync(Promotion promotion, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        dbContext.Promotions.Update(promotion);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(Promotion promotion, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        dbContext.Promotions.Remove(promotion);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var promotionsWithEvents = dbContext.ChangeTracker
            .Entries<Promotion>()
            .Select(entry => entry.Entity)
            .Where(promotion => promotion.DomainEvents.Count > 0)
            .ToArray();

        await dbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        await eventPublisher.PublishAsync(promotionsWithEvents, cancellationToken)
            .ConfigureAwait(false);
    }
}
