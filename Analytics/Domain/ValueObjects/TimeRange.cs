namespace Klippr_Backend.Analytics.Domain.ValueObjects;

public class TimeRange
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public TimeRange(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date cannot be before start date.");

        StartDate = startDate;
        EndDate = endDate;
    }
}