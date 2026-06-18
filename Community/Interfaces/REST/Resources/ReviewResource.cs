namespace Klippr_Backend.Community.Interfaces.REST.Resources;

public record ReviewResource(int Id, string UserId, string PromotionId, string RedemptionId, int Rating, 
    string Status, string Comment, string ReviewId, string Content, string BusinessId);