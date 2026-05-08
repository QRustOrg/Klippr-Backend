using Klippr_Backend.Community.Domain.Model.Commands;
using Klippr_Backend.Community.Interfaces.REST.Resources;

namespace Klippr_Backend.Community.Interfaces.REST.Transform;

public static class CreateReviewCommandFromResourceAssembler
{
    public static CreateReviewCommand ToCommandFromResource(CreateReviewResource resource) =>
        new CreateReviewCommand(
            resource.UserId, 
            resource.PromotionId,
            resource.RedemptionId,
            resource.Rating,
            resource.Status,
            resource.Comment,
            resource.ReviewId,
            resource.Content,
            resource.BusinessId
        );
}