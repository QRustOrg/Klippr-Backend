using Klippr_Backend.Community.Domain.Aggregates;

namespace Klippr_Backend.Community.Interface.Transform;

public static class CommentResourceFromEntityAssembler
{
    public static CommentResource ToResource(ReviewComment comment, string userName)
    {
        ArgumentNullException.ThrowIfNull(comment);

        return new CommentResource(
            comment.Id.ToString(),
            comment.ReviewId.ToString(),
            comment.UserId.ToString(),
            userName,
            comment.Comment,
            comment.CreatedAt);
    }
}
