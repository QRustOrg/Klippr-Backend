namespace Klippr_Backend.IAM.Domain.Queries;

public class GetUsersByRoleQuery
{
    public string Role { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
