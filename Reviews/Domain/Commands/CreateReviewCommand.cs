namespace Klippr_Backend.Reviews.Domain.Commands;

public record CreateReviewCommand(
    Guid UserId,
    Guid PromotionId,
    int Rating,
    string Comment
);
