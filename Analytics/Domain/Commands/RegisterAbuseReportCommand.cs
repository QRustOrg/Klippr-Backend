namespace Klippr_Backend.Analytics.Domain.Commands;

public class RegisterAbuseReportCommand
{
    public Guid ReportedEntityId { get; set; }
    public Guid ReportedBy { get; set; }
    public string Reason { get; set; }
    public string Description { get; set; }

    public RegisterAbuseReportCommand(Guid reportedEntityId, Guid reportedBy, string reason, string description)
    {
        ReportedEntityId = reportedEntityId;
        ReportedBy = reportedBy;
        Reason = reason;
        Description = description;
    }
}