namespace Klippr_Backend.Profile.Domain.ValueObjects;

public class VerificationStatus : IEquatable<VerificationStatus>
{
    public static readonly VerificationStatus Pending = new("PENDING");
    public static readonly VerificationStatus Approved = new("APPROVED");
    public static readonly VerificationStatus Rejected = new("REJECTED");
    public static readonly VerificationStatus None = new("NONE");

    private static readonly VerificationStatus[] ValidStatuses = { Pending, Approved, Rejected, None };

    public string Value { get; }

    private VerificationStatus(string value)
    {
        Value = value;
    }

    public static VerificationStatus Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Status cannot be null or empty.", nameof(value));

        var normalizedValue = value.Trim().ToUpperInvariant();
        var existingStatus = ValidStatuses.FirstOrDefault(s => s.Value == normalizedValue);

        if (existingStatus == null)
            throw new ArgumentException($"Status '{value}' is not valid.", nameof(value));

        return existingStatus;
    }

    public bool IsPending => Value == Pending.Value;
    public bool IsApproved => Value == Approved.Value;
    public bool IsRejected => Value == Rejected.Value;
    public bool IsNone => Value == None.Value;

    public override bool Equals(object? obj) => Equals(obj as VerificationStatus);

    public bool Equals(VerificationStatus? other) => other != null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;

    public static implicit operator string(VerificationStatus status) => status.Value;
}
