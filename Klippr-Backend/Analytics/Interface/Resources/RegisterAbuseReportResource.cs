namespace Klippr_Backend.Analytics.Interface.Resources;

public record RegisterAbuseReportResource(
    Guid ReportedEntityId,
    Guid ReportedBy,
    string Reason,
    string Description
);