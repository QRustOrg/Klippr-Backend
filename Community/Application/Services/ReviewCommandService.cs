using Klippr_Backend.Community.Domain.Aggregates;
using Klippr_Backend.Community.Domain.Commands;
using Klippr_Backend.Community.Domain.Queries;
using Klippr_Backend.Community.Domain.Repositories;
using Klippr_Backend.Community.Domain.Services;

namespace Klippr_Backend.Community.Application.Services;

public class ReviewCommandService(
    IReviewRepository reviewRepository,
    IReviewQueryService reviewQueryService) : IReviewCommandService
{
    public async Task<Review> Handle(CreateReviewCommand command)
    {
        var canReview = await reviewQueryService
            .Handle(new CanReviewQuery(command.PromotionId, command.UserId))
            .ConfigureAwait(false);

        if (!canReview)
            throw new InvalidOperationException("User is not eligible to review this promotion.");

        var review = Review.Create(command);
        await reviewRepository.AddAsync(review).ConfigureAwait(false);
        await reviewRepository.SaveChangesAsync().ConfigureAwait(false);
        return review;
    }

    public async Task<ReviewComment> Handle(AddCommentCommand command)
    {
        var review = await reviewRepository.FindByIdAsync(command.ReviewId).ConfigureAwait(false);
        if (review is null)
            throw new KeyNotFoundException("Review not found.");

        var comment = review.AddComment(command.UserId, command.Comment);
        await reviewRepository.SaveChangesAsync().ConfigureAwait(false);
        return comment;
    }

    public async Task<Review?> Handle(ToggleLikeCommand command)
    {
        var review = await reviewRepository.FindByIdAsync(command.ReviewId).ConfigureAwait(false);
        if (review is null)
            return null;

        review.ToggleLike(command.UserId);
        reviewRepository.Update(review);
        await reviewRepository.SaveChangesAsync().ConfigureAwait(false);
        return review;
    }
}
