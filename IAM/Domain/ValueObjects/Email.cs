namespace Domain.ValueObjects;

public class Email : IEquatable<Email>
{
    private static readonly string[] ReservedDomains = { "test.com", "example.com", "invalid.com" };

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be null or empty.", nameof(value));

        var trimmedEmail = value.Trim().ToLowerInvariant();

        if (!IsValidEmailFormat(trimmedEmail))
            throw new ArgumentException("Email format is invalid.", nameof(value));

        if (IsReservedDomain(trimmedEmail))
            throw new ArgumentException("Email domain is not allowed.", nameof(value));

        return new Email(trimmedEmail);
    }

    private static bool IsValidEmailFormat(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsReservedDomain(string email)
    {
        var domain = email.Split('@')[1];
        return ReservedDomains.Contains(domain);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Email);
    }

    public bool Equals(Email? other)
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

    public static implicit operator string(Email email)
    {
        return email.Value;
    }
}
