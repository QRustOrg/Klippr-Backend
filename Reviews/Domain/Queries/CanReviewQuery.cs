namespace Klippr_Backend.Reviews.Domain.Queries;

public record CanReviewQuery(
    Guid PromotionId,
    Guid UserId
);
