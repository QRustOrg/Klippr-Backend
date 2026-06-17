namespace Klippr_Backend.Reviews.Interface.Transform;

public record CommentResource(
    string Id,
    string ReviewId,
    string UserId,
    string UserName,
    string Comment,
    DateTimeOffset CreatedAt
);
