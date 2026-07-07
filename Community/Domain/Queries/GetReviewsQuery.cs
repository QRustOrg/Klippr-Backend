namespace Klippr_Backend.Community.Domain.Queries;

public record GetReviewsQuery(
    Guid? PromotionId,
    Guid? UserId
);
