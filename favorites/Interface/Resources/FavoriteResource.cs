namespace Klippr_Backend.Favorites.Interface.Resources;

public record FavoriteResource(
    int             Id,
    string          FavoriteId,
    string          UserId,
    string          PromotionId,
    DateTimeOffset? CreatedAt);