namespace Klippr_Backend.Reviews.Domain.Queries;

public record GetCommentsByReviewIdQuery(
    Guid ReviewId
);
