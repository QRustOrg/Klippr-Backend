namespace Klippr_Backend.Profile.Domain.Commands;

public class SubmitBusinessVerificationCommand
{
    public Guid ProfileId { get; set; }
    public string DocumentUrl { get; set; } = string.Empty;
}
