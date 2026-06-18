using System.ComponentModel.DataAnnotations;

namespace Klippr_Backend.Community.Interfaces.REST.Resources;

public record CreateReviewResource(
    [Required] string UserId,
    [Required] string PromotionId,
    [Required] string RedemptionId,
    [Required] int Rating,
    [Required] string Status,
    [Required] string Comment,
    [Required] string ReviewId,
    [Required] string Content,
    [Required] string BusinessId);