namespace Klippr_Backend.Community.Domain.Commands;

public record CreateReviewCommand(
    Guid UserId,
    Guid PromotionId,
    int Rating,
    string Comment
);
