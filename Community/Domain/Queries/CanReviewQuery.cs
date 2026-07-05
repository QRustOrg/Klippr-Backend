namespace Klippr_Backend.Community.Domain.Queries;

public record CanReviewQuery(
    Guid PromotionId,
    Guid UserId
);
