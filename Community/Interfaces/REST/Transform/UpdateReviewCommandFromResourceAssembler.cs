using Klippr_Backend.Community.Domain.Model.Commands;
using Klippr_Backend.Community.Interfaces.REST.Resources;

namespace Klippr_Backend.Community.Interfaces.REST.Transform;

public static class UpdateReviewCommandFromResourceAssembler
{
    public static UpdateReviewCommand ToCommandFromResource(int id, UpdateReviewResource resource) =>
        new UpdateReviewCommand(
            id,
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