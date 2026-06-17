namespace Klippr_Backend.Reviews.Domain.Queries;

public record GetReviewsQuery(
    Guid? PromotionId,
    Guid? UserId
);
