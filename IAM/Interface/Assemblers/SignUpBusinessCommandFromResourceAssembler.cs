using Domain.Commands;
using Interface.Resources;

namespace Interface.Assemblers;

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
