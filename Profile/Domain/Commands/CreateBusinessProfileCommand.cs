namespace Domain.Commands;

public class CreateBusinessProfileCommand
{
    public Guid UserId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
