namespace Klippr_Backend.Analytics.Interface.Resources;

public class PlatformAnalyticsResource
{
    public int TotalUsers { get; set; }
    public int TotalPromotions { get; set; }
    public int TotalAbuseReports { get; set; }
    public int PendingAbuseReports { get; set; }
    public int ReviewedAbuseReports { get; set; }
    public int ResolvedAbuseReports { get; set; }
}
