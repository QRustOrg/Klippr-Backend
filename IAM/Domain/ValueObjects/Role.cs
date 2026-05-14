namespace Klippr_Backend.IAM.Domain.ValueObjects;

public class Role : IEquatable<Role>
{
    public static readonly Role Consumer = new("CONSUMER");
    public static readonly Role Business = new("BUSINESS");
    public static readonly Role Admin = new("ADMIN");

    private static readonly Role[] ValidRoles = { Consumer, Business, Admin };

    public string Value { get; }

    private Role(string value)
    {
        Value = value;
    }

    public static Role Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Role cannot be null or empty.", nameof(value));

        var normalizedValue = value.Trim().ToUpperInvariant();

        var existingRole = ValidRoles.FirstOrDefault(r => r.Value == normalizedValue);

        if (existingRole == null)
            throw new ArgumentException($"Role '{value}' is not valid. Allowed roles: CONSUMER, BUSINESS, ADMIN.", nameof(value));

        return existingRole;
    }

    public bool IsConsumer => Value == Consumer.Value;
    public bool IsBusiness => Value == Business.Value;
    public bool IsAdmin => Value == Admin.Value;

    public override bool Equals(object? obj)
    {
        return Equals(obj as Role);
    }

    public bool Equals(Role? other)
    {
        return other != null && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(Role role)
    {
        return role.Value;
    }
}
