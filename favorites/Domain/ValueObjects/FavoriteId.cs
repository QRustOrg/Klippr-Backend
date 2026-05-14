namespace Klippr_Backend.Favorites.Domain.ValueObjects;

public readonly record struct FavoriteId(string Value)
{
    public static FavoriteId New() => new(Guid.NewGuid().ToString());
    public static FavoriteId Of(string value) => new(value);
    public override string ToString() => Value;
}