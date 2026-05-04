namespace Domain.Queries;

public class GetProfilesWithVerificationPendingQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
