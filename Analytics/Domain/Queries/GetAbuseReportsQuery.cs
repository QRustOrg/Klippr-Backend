using Klippr_Backend.Analytics.Domain.ValueObjects;

namespace Klippr_Backend.Analytics.Domain.Queries;

public record GetAbuseReportsQuery(AbuseReportStatus? Status);