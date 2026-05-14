namespace Klippr_Backend.Analytics.Interface.Resources;

public record AbuseReportResource(
    Guid Id,
    Guid ReportedEntityId,
    Guid ReportedBy,
    string Reason,
    string Description,
    string Status,
    DateTime CreatedAt
);