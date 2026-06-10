using Klippr_Backend.Analytics.Domain.ValueObjects;

namespace Klippr_Backend.Analytics.Domain.Commands;

public record UpdateAbuseReportStatusCommand(Guid ReportId, AbuseReportStatus Status);
