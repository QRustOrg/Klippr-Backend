using Klippr_Backend.Redemption.Domain.Repositories;
using Klippr_Backend.Redemption.Domain.ValueObjects;
using Klippr_Backend.Reviews.Domain.Aggregates;
using Klippr_Backend.Reviews.Domain.Queries;
using Klippr_Backend.Reviews.Domain.Repositories;
using Klippr_Backend.Reviews.Domain.Services;

namespace Klippr_Backend.Reviews.Application.Services;

public class ReviewQueryService(
    IReviewRepository reviewRepository,
    IRedemptionRepository redemptionRepository) : IReviewQueryService
{
    public Task<IEnumerable<Review>> Handle(GetReviewsQuery query) =>
        reviewRepository.FindAllAsync(query.PromotionId, query.UserId);

    public Task<Review?> Handle(GetReviewByIdQuery query) =>
        reviewRepository.FindByIdAsync(query.ReviewId);

    public async Task<IEnumerable<ReviewComment>> Handle(GetCommentsByReviewIdQuery query)
    {
        var review = await reviewRepository.FindByIdAsync(query.ReviewId).ConfigureAwait(false);
        return review?.Comments ?? Enumerable.Empty<ReviewComment>();
    }

    public async Task<bool> Handle(CanReviewQuery query)
    {
        var alreadyReviewed = await reviewRepository
            .ExistsByUserAndPromotionAsync(query.UserId, query.PromotionId)
            .ConfigureAwait(false);

        if (alreadyReviewed)
            return false;

        return await HasRedeemedPromotionAsync(query.UserId, query.PromotionId).ConfigureAwait(false);
    }

    public async Task<bool> HasRedeemedPromotionAsync(Guid userId, Guid promotionId)
    {
        // ponytail: Redemption.PromotionId is string (legacy typing), Review.PromotionId is Guid (correct).
        // Cast here instead of touching the Redemption module or adding a new repository method.
        var promotionIdAsString = promotionId.ToString();
        var redemptions = await redemptionRepository.FindByConsumerIdAsync(userId).ConfigureAwait(false);

        return redemptions.Any(redemption =>
            redemption.PromotionId == promotionIdAsString &&
            redemption.Status == RedemptionStatus.Redeemed);
    }
}
