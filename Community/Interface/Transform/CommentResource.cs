namespace Klippr_Backend.Community.Interface.Transform;

public record CommentResource(
    string Id,
    string ReviewId,
    string UserId,
    string UserName,
    string Comment,
    DateTimeOffset CreatedAt
);
