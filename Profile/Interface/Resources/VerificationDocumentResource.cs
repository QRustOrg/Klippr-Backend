namespace Klippr_Backend.Profile.Interface.Resources;

public class VerificationDocumentResource
{
    public Guid ProfileId { get; set; }
    public string DocumentUrl { get; set; } = string.Empty;
}
