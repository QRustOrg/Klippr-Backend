namespace Klippr_Backend.Favorites.Domain.ValueObjects;

public readonly record struct UserId(string Value)
{
    public static UserId Of(string value) => new(value);
    public override string ToString() => Value;
}