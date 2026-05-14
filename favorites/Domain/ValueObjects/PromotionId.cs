namespace Klippr_Backend.Favorites.Domain.ValueObjects;

public readonly record struct PromotionId(string Value)
{
    public static PromotionId Of(string value) => new(value);
    public override string ToString() => Value;
}