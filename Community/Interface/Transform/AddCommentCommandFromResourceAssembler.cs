using Klippr_Backend.Community.Domain.Commands;

namespace Klippr_Backend.Community.Interface.Transform;

public static class AddCommentCommandFromResourceAssembler
{
    public static AddCommentCommand ToCommand(Guid reviewId, Guid userId, AddCommentResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new AddCommentCommand(reviewId, userId, resource.Comment);
    }
}
