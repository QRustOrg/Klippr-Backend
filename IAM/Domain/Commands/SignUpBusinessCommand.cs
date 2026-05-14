namespace Klippr_Backend.IAM.Domain.Commands;

public class SignUpBusinessCommand
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
}
