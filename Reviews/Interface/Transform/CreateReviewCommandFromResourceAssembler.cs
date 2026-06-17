using Klippr_Backend.Reviews.Domain.Commands;

namespace Klippr_Backend.Reviews.Interface.Transform;

public static class CreateReviewCommandFromResourceAssembler
{
    public static CreateReviewCommand ToCommand(Guid userId, CreateReviewResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new CreateReviewCommand(userId, resource.PromotionId, resource.Rating, resource.Comment);
    }
}
