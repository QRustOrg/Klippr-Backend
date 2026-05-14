namespace Klippr_Backend.IAM.Domain.Commands;

public class SignInCommand
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
