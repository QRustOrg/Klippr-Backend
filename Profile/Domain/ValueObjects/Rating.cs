namespace Klippr_Backend.Profile.Domain.ValueObjects;

public class Rating : IEquatable<Rating>
{
    public double AverageRating { get; }
    public int TotalReviews { get; }
    public int FiveStarCount { get; }
    public int FourStarCount { get; }
    public int ThreeStarCount { get; }
    public int TwoStarCount { get; }
    public int OneStarCount { get; }

    private Rating(double averageRating, int totalReviews, int fiveStarCount, int fourStarCount, int threeStarCount, int twoStarCount, int oneStarCount)
    {
        AverageRating = averageRating;
        TotalReviews = totalReviews;
        FiveStarCount = fiveStarCount;
        FourStarCount = fourStarCount;
        ThreeStarCount = threeStarCount;
        TwoStarCount = twoStarCount;
        OneStarCount = oneStarCount;
    }

    public static Rating Create(double averageRating, int totalReviews, int fiveStarCount, int fourStarCount, int threeStarCount, int twoStarCount, int oneStarCount)
    {
        if (averageRating < 0 || averageRating > 5)
            throw new ArgumentException("Average rating must be between 0 and 5.", nameof(averageRating));
        if (totalReviews < 0)
            throw new ArgumentException("Total reviews cannot be negative.", nameof(totalReviews));
        if (fiveStarCount < 0 || fourStarCount < 0 || threeStarCount < 0 || twoStarCount < 0 || oneStarCount < 0)
            throw new ArgumentException("Star counts cannot be negative.");

        return new Rating(averageRating, totalReviews, fiveStarCount, fourStarCount, threeStarCount, twoStarCount, oneStarCount);
    }

    public static Rating Empty() => new(0, 0, 0, 0, 0, 0, 0);

    public override bool Equals(object? obj) => Equals(obj as Rating);

    public bool Equals(Rating? other) =>
        other != null &&
        AverageRating == other.AverageRating &&
        TotalReviews == other.TotalReviews;

    public override int GetHashCode() => HashCode.Combine(AverageRating, TotalReviews);

    public override string ToString() => $"{AverageRating:F1} ({TotalReviews} reviews)";
}
