using Klippr_Backend.Profile.Domain.ValueObjects;

namespace Klippr_Backend.Profile.Domain.Aggregates;

public class ConsumerProfile
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? PhoneNumber { get; private set; }
    public Location? Location { get; private set; }
    public Rating? Rating { get; private set; }
    public SavingsStatistics? SavingsStatistics { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private ConsumerProfile() { }

    public static ConsumerProfile Create(Guid userId, string firstName, string lastName, string? phoneNumber = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        return new ConsumerProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            PhoneNumber = phoneNumber?.Trim(),
            Rating = Rating.Empty(),
            SavingsStatistics = SavingsStatistics.Empty(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void UpdateProfile(string firstName, string lastName, string? phoneNumber = null, Location? location = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PhoneNumber = phoneNumber?.Trim();
        if (location != null)
            Location = location;

        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRating(Rating rating)
    {
        if (rating == null)
            throw new ArgumentNullException(nameof(rating));

        Rating = rating;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateSavingsStatistics(SavingsStatistics statistics)
    {
        if (statistics == null)
            throw new ArgumentNullException(nameof(statistics));

        SavingsStatistics = statistics;
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
