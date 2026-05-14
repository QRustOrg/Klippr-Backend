using Klippr_Backend.IAM.Domain.Commands;
using Klippr_Backend.IAM.Interface.Resources;

namespace Klippr_Backend.IAM.Interface.Assemblers;

public class SignUpBusinessCommandFromResourceAssembler
{
    public static SignUpBusinessCommand Assemble(SignUpBusinessResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        return new SignUpBusinessCommand
        {
            Email = resource.Email,
            Password = resource.Password,
            BusinessName = resource.BusinessName,
            TaxId = resource.TaxId
        };
    }
}
