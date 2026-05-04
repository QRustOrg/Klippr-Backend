namespace Domain.Events;

public abstract class DomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

public class UserCreatedEvent : DomainEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class UserSignedInEvent : DomainEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime SignedInAt { get; set; }
}

public class UserDeactivatedEvent : DomainEvent
{
    public Guid UserId { get; set; }
    public DateTime DeactivatedAt { get; set; }
}
