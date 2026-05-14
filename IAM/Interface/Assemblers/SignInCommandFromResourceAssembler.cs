using Klippr_Backend.IAM.Domain.Commands;
using Klippr_Backend.IAM.Interface.Resources;

namespace Klippr_Backend.IAM.Interface.Assemblers;

public class SignInCommandFromResourceAssembler
{
    public static SignInCommand Assemble(SignInResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        return new SignInCommand
        {
            Email = resource.Email,
            Password = resource.Password
        };
    }
}
