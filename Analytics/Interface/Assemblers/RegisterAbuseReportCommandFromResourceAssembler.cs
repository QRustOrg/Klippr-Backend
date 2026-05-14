using Klippr_Backend.Analytics.Domain.Commands;
using Klippr_Backend.Analytics.Interface.Resources;

namespace Klippr_Backend.Analytics.Interface.Assemblers;

public static class RegisterAbuseReportCommandFromResourceAssembler
{
    public static RegisterAbuseReportCommand ToCommand(RegisterAbuseReportResource resource)
    {
        return new RegisterAbuseReportCommand(
            resource.ReportedEntityId,
            resource.ReportedBy,
            resource.Reason,
            resource.Description
        );
    }
}  
