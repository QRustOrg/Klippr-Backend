using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;
using Klippr_Backend.Favorites.Domain.Commands;
using Klippr_Backend.Favorites.Domain.ValueObjects;

namespace Klippr_Backend.Favorites.Domain.Aggregates;

public class Favorite : IEntityWithCreatedUpdatedDate
{
    private Favorite()
    {
        FavoriteId  = string.Empty;
        UserId      = string.Empty;
        PromotionId = string.Empty;
    }

    public Favorite(SaveFavoriteCommand command)
    {
        FavoriteId  = ValueObjects.FavoriteId.New().Value;
        UserId      = command.UserId;
        PromotionId = command.PromotionId;
    }

    public static Favorite Create(UserId userId, PromotionId promotionId) =>
        new()
        {
            FavoriteId  = ValueObjects.FavoriteId.New().Value,
            UserId      = userId.Value,
            PromotionId = promotionId.Value
        };

    public bool BelongsToUser(string userId) =>
        string.Equals(UserId, userId, StringComparison.Ordinal);

    public int    Id          { get; private set; }
    public string FavoriteId  { get; private set; }
    public string UserId      { get; private set; }
    public string PromotionId { get; private set; }

    [Column("CreatedAt")] public DateTimeOffset? CreatedDate { get; set; }
    [Column("UpdatedAt")] public DateTimeOffset? UpdatedDate { get; set; }
}