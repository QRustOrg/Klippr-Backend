using Klippr_Backend.Reviews.Domain.Aggregates;

namespace Klippr_Backend.Reviews.Interface.Transform;

public static class ReviewResourceFromEntityAssembler
{
    public static ReviewResource ToResource(
        Review review,
        string promotionTitle,
        string? promotionImage,
        string businessName,
        string userName,
        string? userAvatar,
        bool verified,
        bool likedByCurrentUser)
    {
        ArgumentNullException.ThrowIfNull(review);

        return new ReviewResource(
            review.Id.ToString(),
            review.PromotionId.ToString(),
            promotionTitle,
            promotionImage,
            businessName,
            review.UserId.ToString(),
            userName,
            userAvatar,
            review.Rating,
            review.Comment,
            review.CreatedAt.ToUnixTimeMilliseconds(),
            verified,
            review.LikeCount,
            likedByCurrentUser);
    }
}
