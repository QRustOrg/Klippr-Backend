namespace Klippr_Backend.Community.Domain.Model.Commands;

public record CreateReviewCommand(string UserId, string PromotionId, string RedemptionId, int Rating,
    string Status, string Comment, string ReviewId, string Content, string BusinessId);