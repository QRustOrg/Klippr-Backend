using Klippr_Backend.Redemption.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Redemption.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementa el repositorio de canjes usando EF Core.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// La implementacion se limita a persistir y recuperar agregados, sin aplicar reglas de negocio.
/// </remarks>
public class RedemptionRepository(RedemptionDbContext dbContext) : IRedemptionRepository
{
    /// <inheritdoc />
    public Task<RedemptionAggregate?> FindByIdAsync(int id)
    {
        return dbContext.Redemptions
            .AsNoTracking()
            .FirstOrDefaultAsync(redemption => redemption.Id == id);
    }

    /// <inheritdoc />
    public Task<RedemptionAggregate?> FindByUniqueTokenAsync(Guid uniqueToken)
    {
        return dbContext.Redemptions
            .AsNoTracking()
            .FirstOrDefaultAsync(redemption => redemption.UniqueToken == uniqueToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RedemptionAggregate>> FindByConsumerIdAsync(Guid consumerId)
    {
        return await dbContext.Redemptions
            .AsNoTracking()
            .Where(redemption => redemption.ConsumerId == consumerId)
            .OrderByDescending(redemption => redemption.GeneratedAt)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RedemptionAggregate>> FindByBusinessIdAsync(Guid businessId)
    {
        return await dbContext.Redemptions
            .AsNoTracking()
            .Where(redemption => redemption.BusinessId == businessId)
            .OrderByDescending(redemption => redemption.GeneratedAt)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddAsync(RedemptionAggregate redemption)
    {
        ArgumentNullException.ThrowIfNull(redemption);

        await dbContext.Redemptions
            .AddAsync(redemption)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public void Update(RedemptionAggregate redemption)
    {
        ArgumentNullException.ThrowIfNull(redemption);

        dbContext.Redemptions.Update(redemption);
    }

    /// <inheritdoc />
    public void Remove(RedemptionAggregate redemption)
    {
        ArgumentNullException.ThrowIfNull(redemption);

        dbContext.Redemptions.Remove(redemption);
    }
}
