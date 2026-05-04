namespace Domain.Commands;

public class UpdateBusinessProfileCommand
{
    public Guid ProfileId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
}
