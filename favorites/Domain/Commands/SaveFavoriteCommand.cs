namespace Klippr_Backend.Favorites.Domain.Commands;

public record SaveFavoriteCommand(string UserId, string PromotionId);