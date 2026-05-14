namespace Klippr_Backend.Profile.Domain.ValueObjects;

public class BusinessCategory : IEquatable<BusinessCategory>
{
    public static readonly BusinessCategory Restaurant = new("RESTAURANT");
    public static readonly BusinessCategory Retail = new("RETAIL");
    public static readonly BusinessCategory Services = new("SERVICES");
    public static readonly BusinessCategory Entertainment = new("ENTERTAINMENT");
    public static readonly BusinessCategory Health = new("HEALTH");
    public static readonly BusinessCategory Other = new("OTHER");

    private static readonly BusinessCategory[] ValidCategories = { Restaurant, Retail, Services, Entertainment, Health, Other };

    public string Value { get; }

    private BusinessCategory(string value)
    {
        Value = value;
    }

    public static BusinessCategory Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Category cannot be null or empty.", nameof(value));

        var normalizedValue = value.Trim().ToUpperInvariant();
        var existingCategory = ValidCategories.FirstOrDefault(c => c.Value == normalizedValue);

        if (existingCategory == null)
            throw new ArgumentException($"Category '{value}' is not valid.", nameof(value));

        return existingCategory;
    }

    public override bool Equals(object? obj) => Equals(obj as BusinessCategory);

    public bool Equals(BusinessCategory? other) => other != null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;

    public static implicit operator string(BusinessCategory category) => category.Value;
}
