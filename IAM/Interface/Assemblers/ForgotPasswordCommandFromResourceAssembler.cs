using Klippr_Backend.IAM.Domain.Commands;
using Klippr_Backend.IAM.Interface.Resources;

namespace Klippr_Backend.IAM.Interface.Assemblers;

public class ForgotPasswordCommandFromResourceAssembler
{
    public static ForgotPasswordCommand Assemble(ForgotPasswordResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        return new ForgotPasswordCommand
        {
            Email = resource.Email
        };
    }
}
