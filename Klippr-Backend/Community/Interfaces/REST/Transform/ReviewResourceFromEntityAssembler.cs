using Klippr_Backend.Community.Domain.Model.Aggregate;
using Klippr_Backend.Community.Interfaces.REST.Resources;

namespace Klippr_Backend.Community.Interfaces.REST.Transform;

public static class ReviewResourceFromEntityAssembler
{
    public static ReviewResource ToResourceFromEntity(Review entity) =>
        new ReviewResource(
            entity.Id, 
            entity.UserId, 
            entity.PromotionId,
            entity.RedemptionId,
            entity.Rating,
            entity.Status,
            entity.Comment,
            entity.ReviewId,
            entity.Content,
            entity.BusinessId
        );
}