using Klippr_Backend.IAM.Domain.Commands;
using Klippr_Backend.IAM.Interface.Resources;

namespace Klippr_Backend.IAM.Interface.Assemblers;

public class ResetPasswordCommandFromResourceAssembler
{
    public static ResetPasswordCommand Assemble(ResetPasswordResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        return new ResetPasswordCommand
        {
            Email = resource.Email,
            NewPassword = resource.NewPassword
        };
    }
}
