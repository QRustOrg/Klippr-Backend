namespace Klippr_Backend.Reviews.Interface.Transform;

public record CreateReviewResource(Guid PromotionId, int Rating, string Comment);
