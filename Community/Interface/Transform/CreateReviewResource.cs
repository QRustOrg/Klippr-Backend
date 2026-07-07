namespace Klippr_Backend.Community.Interface.Transform;

public record CreateReviewResource(Guid PromotionId, int Rating, string Comment);
