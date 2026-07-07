namespace Klippr_Backend.Favorites.Domain.Commands;

public record RestoreFavoriteCommand(string UserId, string FavoriteId);
