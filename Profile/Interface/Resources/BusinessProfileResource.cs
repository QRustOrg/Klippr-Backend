namespace Klippr_Backend.Profile.Interface.Resources;

public class BusinessProfileResource
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string? Description { get; set; }
    public BusinessCategoryResource? Category { get; set; }
    public LocationResource? Location { get; set; }
    public string VerificationStatus { get; set; } = string.Empty;
    public BusinessRatingResource? Rating { get; set; }
    public string? DocumentUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class BusinessCategoryResource
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class BusinessRatingResource
{
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
}
