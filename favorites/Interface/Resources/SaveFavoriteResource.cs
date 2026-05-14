using System.ComponentModel.DataAnnotations;

namespace Klippr_Backend.Favorites.Interface.Resources;

public record SaveFavoriteResource(
    [Required] string UserId,
    [Required] string PromotionId);