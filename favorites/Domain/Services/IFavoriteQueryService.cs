using Klippr_Backend.Favorites.Domain.Aggregates;
using Klippr_Backend.Favorites.Domain.Queries;

namespace Klippr_Backend.Favorites.Domain.Services;

public interface IFavoriteQueryService
{
    Task<IEnumerable<Favorite>> Handle(GetUserFavoritesQuery query);
    Task<Favorite?>             Handle(int id);
}