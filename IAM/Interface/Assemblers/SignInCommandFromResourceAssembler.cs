using Domain.Commands;
using Interface.Resources;

namespace Interface.Assemblers;

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
