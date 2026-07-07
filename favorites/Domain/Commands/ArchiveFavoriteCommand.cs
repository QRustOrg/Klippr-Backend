namespace Klippr_Backend.Favorites.Domain.Commands;

public record ArchiveFavoriteCommand(string UserId, string FavoriteId);
