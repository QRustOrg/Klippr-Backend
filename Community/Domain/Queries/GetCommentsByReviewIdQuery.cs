namespace Klippr_Backend.Community.Domain.Queries;

public record GetCommentsByReviewIdQuery(
    Guid ReviewId
);
