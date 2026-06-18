using Klippr_Backend.Community.Domain.Model.Commands;

namespace Klippr_Backend.Community.Domain.Model.Aggregate;

public partial class Review
{
    private Review()
    {
        UserId = string.Empty;
        PromotionId = string.Empty;
        RedemptionId = string.Empty;
        Rating = 0;
        Status = string.Empty;
        Comment = string.Empty;
        ReviewId = string.Empty;
        Content = string.Empty;
        BusinessId = string.Empty;
    }
    
    private Review(string userId, string promotionId, string redemptionId, int rating, string status, 
        string comment, string reviewId, string content, string businessId)
    {
        UserId = userId;
        PromotionId = promotionId;
        RedemptionId = redemptionId;
        Rating = rating;
        Status = status;
        Comment = comment;
        ReviewId = reviewId;
        Content = content;
        BusinessId = businessId;
    }
  
    public Review(CreateReviewCommand command)
    {
        UserId = command.UserId;
        PromotionId = command.PromotionId;
        RedemptionId = command.RedemptionId;
        Rating = command.Rating;
        Status = command.Status;
        Comment = command.Comment;
        ReviewId = command.ReviewId;
        Content = command.Content;
        BusinessId = command.BusinessId;
    }
    
    public void UpdateReview(UpdateReviewCommand command)
    {
        UserId = command.UserId;
        PromotionId = command.PromotionId;
        RedemptionId = command.RedemptionId;
        Rating = command.Rating;
        Status = command.Status;
        Comment = command.Comment;
        ReviewId = command.ReviewId;
        Content = command.Content;
        BusinessId = command.BusinessId;
    }
    
    public int Id { get; private set; }
    public string UserId { get; private set; }
    public string PromotionId { get; private set; }
    public string RedemptionId { get; private set; }
    public int Rating { get; private set; }
    public string Status { get; private set; }
    public string Comment { get; private set; }
    public string ReviewId { get; private set; }
    public string Content { get; private set; }
    public string BusinessId { get; private set; }
}
