namespace Klippr_Backend.Profile.Domain.Events;

public abstract class DomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

public class ProfileUpdated : DomainEvent
{
    public Guid ProfileId { get; set; }
    public Guid UserId { get; set; }
    public string ProfileType { get; set; } = string.Empty; // CONSUMER or BUSINESS
    public DateTime UpdatedAt { get; set; }
}

public class VerificationDocumentSubmitted : DomainEvent
{
    public Guid ProfileId { get; set; }
    public string DocumentUrl { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
}

public class VerificationApproved : DomainEvent
{
    public Guid ProfileId { get; set; }
    public DateTime ApprovedAt { get; set; }
}
