using System.Text.Json.Serialization;

namespace Klippr_Backend.Community.Interface.Transform;

public record ReviewResource(
    string Id,
    string PromotionId,
    string PromotionTitle,
    [property: JsonPropertyName("promotionImage")] string? PromotionImage,
    string BusinessName,
    string UserId,
    string UserName,
    [property: JsonPropertyName("userAvatar")] string? UserAvatar,
    int Rating,
    string Comment,
    long CreatedAt,
    bool Verified,
    int LikeCount,
    bool LikedByCurrentUser
);
