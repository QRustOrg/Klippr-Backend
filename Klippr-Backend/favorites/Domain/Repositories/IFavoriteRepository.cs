using Klippr_Backend.Favorites.Domain.Aggregates;
using Klippr_Backend.Shared.Domain.Repositories;

namespace Klippr_Backend.Favorites.Domain.Repositories;

public interface IFavoriteRepository : IBaseRepository<Favorite>
{
    Task<IEnumerable<Favorite>> FindByUserIdAsync(string userId);
    Task<Favorite?>             FindByFavoriteIdAsync(string favoriteId);
    Task<bool>                  ExistsAsync(string userId, string promotionId);
}