using Klippr_Backend.Favorites.Domain.Commands;
using Klippr_Backend.Favorites.Interface.Resources;

namespace Klippr_Backend.Favorites.Interface.Assemblers;

public static class SaveFavoriteCommandFromResourceAssembler
{
    public static SaveFavoriteCommand ToCommandFromResource(SaveFavoriteResource resource) =>
        new(resource.UserId, resource.PromotionId);
}