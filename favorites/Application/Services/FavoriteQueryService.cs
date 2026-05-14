using Klippr_Backend.Favorites.Domain.Aggregates;
using Klippr_Backend.Favorites.Domain.Queries;
using Klippr_Backend.Favorites.Domain.Repositories;
using Klippr_Backend.Favorites.Domain.Services;

namespace Klippr_Backend.Favorites.Application.Services;

public class FavoriteQueryService(IFavoriteRepository favoriteRepository)
    : IFavoriteQueryService
{
    public async Task<IEnumerable<Favorite>> Handle(GetUserFavoritesQuery query) =>
        await favoriteRepository.FindByUserIdAsync(query.UserId);

    public async Task<Favorite?> Handle(int id) =>
        await favoriteRepository.FindByIdAsync(id);
}