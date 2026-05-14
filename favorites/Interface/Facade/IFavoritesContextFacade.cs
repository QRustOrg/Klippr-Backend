namespace Klippr_Backend.Favorites.Interface.Facade;

public interface IFavoritesContextFacade
{
    Task<bool>                        IsFavoriteAsync(string userId, string promotionId);
    Task<int>                         CountByUserAsync(string userId);
    Task<IReadOnlyCollection<string>> GetFavoritePromotionIdsAsync(string userId);
}