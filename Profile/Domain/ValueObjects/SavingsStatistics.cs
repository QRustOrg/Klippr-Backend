namespace Klippr_Backend.Profile.Domain.ValueObjects;

public class SavingsStatistics : IEquatable<SavingsStatistics>
{
    public decimal TotalSavings { get; }
    public int PromotionsUsed { get; }
    public int PromotionsSaved { get; }
    public DateTime LastUpdated { get; }

    private SavingsStatistics(decimal totalSavings, int promotionsUsed, int promotionsSaved, DateTime lastUpdated)
    {
        TotalSavings = totalSavings;
        PromotionsUsed = promotionsUsed;
        PromotionsSaved = promotionsSaved;
        LastUpdated = lastUpdated;
    }

    public static SavingsStatistics Create(decimal totalSavings, int promotionsUsed, int promotionsSaved)
    {
        if (totalSavings < 0)
            throw new ArgumentException("Total savings cannot be negative.", nameof(totalSavings));
        if (promotionsUsed < 0)
            throw new ArgumentException("Promotions used cannot be negative.", nameof(promotionsUsed));
        if (promotionsSaved < 0)
            throw new ArgumentException("Promotions saved cannot be negative.", nameof(promotionsSaved));

        return new SavingsStatistics(totalSavings, promotionsUsed, promotionsSaved, DateTime.UtcNow);
    }

    public static SavingsStatistics Empty() => new(0, 0, 0, DateTime.UtcNow);

    public override bool Equals(object? obj) => Equals(obj as SavingsStatistics);

    public bool Equals(SavingsStatistics? other) =>
        other != null &&
        TotalSavings == other.TotalSavings &&
        PromotionsUsed == other.PromotionsUsed &&
        PromotionsSaved == other.PromotionsSaved;

    public override int GetHashCode() => HashCode.Combine(TotalSavings, PromotionsUsed, PromotionsSaved);

    public override string ToString() => $"${TotalSavings:F2} saved from {PromotionsUsed} promotions";
}
