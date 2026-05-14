namespace Klippr_Backend.Favorites.Interface.Resources;

public record FavoriteListResource(
    string                                UserId,
    int                                   Count,
    IReadOnlyCollection<FavoriteResource> Items);