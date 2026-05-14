using Klippr_Backend.Profile.Domain.Commands;
using Klippr_Backend.Profile.Interface.Resources;

namespace Klippr_Backend.Profile.Interface.Assemblers;

public class VerificationDocumentCommandFromResourceAssembler
{
    public static SubmitBusinessVerificationCommand ToSubmitCommand(VerificationDocumentResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        return new SubmitBusinessVerificationCommand
        {
            ProfileId = resource.ProfileId,
            DocumentUrl = resource.DocumentUrl
        };
    }

    public static ApproveBusinessVerificationCommand ToApproveCommand(Guid profileId)
    {
        if (profileId == Guid.Empty)
            throw new ArgumentException("Profile ID cannot be empty.", nameof(profileId));

        return new ApproveBusinessVerificationCommand
        {
            ProfileId = profileId
        };
    }
}
