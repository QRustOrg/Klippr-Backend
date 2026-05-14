namespace Klippr_Backend.Favorites.Domain.Commands;

public record RemoveFavoriteCommand(string UserId, string FavoriteId);