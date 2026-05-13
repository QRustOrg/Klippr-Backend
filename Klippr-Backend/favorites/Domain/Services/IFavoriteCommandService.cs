using Klippr_Backend.Favorites.Domain.Aggregates;
using Klippr_Backend.Favorites.Domain.Commands;

namespace Klippr_Backend.Favorites.Domain.Services;

public interface IFavoriteCommandService
{
    Task<Favorite?> Handle(SaveFavoriteCommand command);
    Task<bool>      Handle(RemoveFavoriteCommand command);
}