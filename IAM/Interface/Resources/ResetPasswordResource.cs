namespace Klippr_Backend.IAM.Interface.Resources;

public class ResetPasswordResource
{
    public string Email { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
