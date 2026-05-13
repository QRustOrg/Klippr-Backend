using Klippr_Backend.Analytics.Domain.ValueObjects;

namespace Klippr_Backend.Analytics.Domain.Aggregates;

public class AbuseReport
{
    public AbuseReportId Id { get; private set; } = null!;
    public Guid ReportedEntityId { get; private set; }
    public Guid ReportedBy { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public AbuseReportStatus Status { get; private set; }

    protected AbuseReport() { }

    public AbuseReport(Guid reportedEntityId, Guid reportedBy, string reason, string description)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required");

        Id = new AbuseReportId(Guid.NewGuid());
        ReportedEntityId = reportedEntityId;
        ReportedBy = reportedBy;
        Reason = reason;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        Status = AbuseReportStatus.PENDING;
    }

    public void UpdateStatus(AbuseReportStatus status)
    {
        Status = status;
    }
}
