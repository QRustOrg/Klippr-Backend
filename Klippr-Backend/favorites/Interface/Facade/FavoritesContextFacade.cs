using Klippr_Backend.Favorites.Domain.Repositories;

namespace Klippr_Backend.Favorites.Interface.Facade;

public class FavoritesContextFacade(IFavoriteRepository favoriteRepository)
    : IFavoritesContextFacade
{
    public Task<bool> IsFavoriteAsync(string userId, string promotionId) =>
        favoriteRepository.ExistsAsync(userId, promotionId);

    public async Task<int> CountByUserAsync(string userId) =>
        (await favoriteRepository.FindByUserIdAsync(userId)).Count();

    public async Task<IReadOnlyCollection<string>> GetFavoritePromotionIdsAsync(string userId)
    {
        var favorites = await favoriteRepository.FindByUserIdAsync(userId);
        return favorites.Select(f => f.PromotionId).ToList().AsReadOnly();
    }
}