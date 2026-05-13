using Klippr_Backend.Promotions.Domain.ValueObjects;

namespace Klippr_Backend.Promotions.Interface.Transform;

internal static class DiscountTypeParser
{
    public static DiscountType Parse(string? discountType)
    {
        if (string.IsNullOrWhiteSpace(discountType))
            throw new ArgumentException("Discount type is invalid.", nameof(discountType));

        var normalizedDiscountType = discountType
            .Trim()
            .Replace("_", string.Empty, StringComparison.Ordinal)
            .Replace("-", string.Empty, StringComparison.Ordinal)
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .ToUpperInvariant();

        return normalizedDiscountType switch
        {
            "PERCENTAGE" or "PERCENT" => DiscountType.Percentage,
            "FIXEDAMOUNT" or "FIXED" or "AMOUNT" => DiscountType.FixedAmount,
            _ => throw new ArgumentException("Discount type is invalid.", nameof(discountType))
        };
    }
}
