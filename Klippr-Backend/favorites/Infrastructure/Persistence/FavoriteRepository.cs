using Klippr_Backend.Favorites.Domain.Aggregates;
using Klippr_Backend.Favorites.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Favorites.Infrastructure.Persistence;

/// <summary>
/// MySQL / EF Core implementation of <see cref="IFavoriteRepository"/>.
/// <para>
/// Uses <see cref="FavoritesDbContext"/> (own context) and implements all CRUD
/// directly — does NOT extend BaseRepository from Shared.
/// </para>
/// </summary>
public class FavoriteRepository(FavoritesDbContext context) : IFavoriteRepository
{
    // ── CRUD ──────────────────────────────────────────────────────────────
    public async Task AddAsync(Favorite entity) =>
        await context.Set<Favorite>().AddAsync(entity);

    public async Task<Favorite?> FindByIdAsync(int id) =>
        await context.Set<Favorite>().FindAsync(id);

    public void Update(Favorite entity) =>
        context.Set<Favorite>().Update(entity);

    public void Remove(Favorite entity) =>
        context.Set<Favorite>().Remove(entity);

    public async Task<IEnumerable<Favorite>> ListAsync() =>
        await context.Set<Favorite>().ToListAsync();

    // ── Favorites-specific ────────────────────────────────────────────────
    public async Task<IEnumerable<Favorite>> FindByUserIdAsync(string userId) =>
        await context.Set<Favorite>()
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();

    public async Task<Favorite?> FindByFavoriteIdAsync(string favoriteId) =>
        await context.Set<Favorite>()
            .FirstOrDefaultAsync(f => f.FavoriteId == favoriteId);

    public async Task<bool> ExistsAsync(string userId, string promotionId) =>
        await context.Set<Favorite>()
            .AnyAsync(f => f.UserId == userId && f.PromotionId == promotionId);
}