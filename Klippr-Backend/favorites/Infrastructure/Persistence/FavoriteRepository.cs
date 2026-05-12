using Klippr_Backend.Favorites.Domain.Aggregates;
using Klippr_Backend.Favorites.Domain.Repositories;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Favorites.Infrastructure.Persistence;

public class FavoriteRepository(AppDbContext context)
    : BaseRepository<Favorite>(context), IFavoriteRepository
{
    public async Task<IEnumerable<Favorite>> FindByUserIdAsync(string userId) =>
        await Context.Set<Favorite>()
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();

    public async Task<Favorite?> FindByFavoriteIdAsync(string favoriteId) =>
        await Context.Set<Favorite>()
            .FirstOrDefaultAsync(f => f.FavoriteId == favoriteId);

    public async Task<bool> ExistsAsync(string userId, string promotionId) =>
        await Context.Set<Favorite>()
            .AnyAsync(f => f.UserId == userId && f.PromotionId == promotionId);
}