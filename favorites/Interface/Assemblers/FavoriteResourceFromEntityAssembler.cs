using Klippr_Backend.Favorites.Domain.Aggregates;
using Klippr_Backend.Favorites.Interface.Resources;

namespace Klippr_Backend.Favorites.Interface.Assemblers;

public static class FavoriteResourceFromEntityAssembler
{
    public static FavoriteResource ToResourceFromEntity(Favorite entity) =>
        new(entity.Id, entity.FavoriteId, entity.UserId, entity.PromotionId, entity.CreatedDate);
}