using Klippr_Backend.IAM.Domain.Commands;
using Klippr_Backend.IAM.Interface.Resources;

namespace Klippr_Backend.IAM.Interface.Assemblers;

public class SignUpAdminCommandFromResourceAssembler
{
    public static SignUpAdminCommand Assemble(SignUpAdminResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        return new SignUpAdminCommand
        {
            Email = resource.Email,
            Password = resource.Password,
            FirstName = resource.FirstName,
            LastName = resource.LastName
        };
    }
}
