using Domain.ValueObjects;

namespace Domain.Aggregates;

public class BusinessProfile
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string BusinessName { get; private set; }
    public string TaxId { get; private set; }
    public BusinessCategory Category { get; private set; }
    public Location? Location { get; private set; }
    public string? Description { get; private set; }
    public VerificationStatus VerificationStatus { get; private set; }
    public string? VerificationDocumentUrl { get; private set; }
    public DateTime? VerificationSubmittedAt { get; private set; }
    public DateTime? VerificationApprovedAt { get; private set; }
    public Rating? Rating { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private BusinessProfile() { }

    public static BusinessProfile Create(Guid userId, string businessName, string taxId, BusinessCategory category)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        if (string.IsNullOrWhiteSpace(businessName))
            throw new ArgumentException("Business name cannot be null or empty.", nameof(businessName));
        if (string.IsNullOrWhiteSpace(taxId))
            throw new ArgumentException("Tax ID cannot be null or empty.", nameof(taxId));
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        return new BusinessProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            BusinessName = businessName.Trim(),
            TaxId = taxId.Trim(),
            Category = category,
            VerificationStatus = VerificationStatus.None,
            Rating = Rating.Empty(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void UpdateProfile(string businessName, BusinessCategory category, string? description = null, Location? location = null)
    {
        if (string.IsNullOrWhiteSpace(businessName))
            throw new ArgumentException("Business name cannot be null or empty.", nameof(businessName));
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        BusinessName = businessName.Trim();
        Category = category;
        Description = description?.Trim();
        if (location != null)
            Location = location;

        UpdatedAt = DateTime.UtcNow;
    }

    public void SubmitVerification(string documentUrl)
    {
        if (string.IsNullOrWhiteSpace(documentUrl))
            throw new ArgumentException("Document URL cannot be null or empty.", nameof(documentUrl));

        VerificationStatus = VerificationStatus.Pending;
        VerificationDocumentUrl = documentUrl.Trim();
        VerificationSubmittedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ApproveVerification()
    {
        if (!VerificationStatus.IsPending)
            throw new InvalidOperationException("Only pending verifications can be approved.");

        VerificationStatus = VerificationStatus.Approved;
        VerificationApprovedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RejectVerification()
    {
        if (!VerificationStatus.IsPending)
            throw new InvalidOperationException("Only pending verifications can be rejected.");

        VerificationStatus = VerificationStatus.Rejected;
        VerificationDocumentUrl = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRating(Rating rating)
    {
        if (rating == null)
            throw new ArgumentNullException(nameof(rating));

        Rating = rating;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
