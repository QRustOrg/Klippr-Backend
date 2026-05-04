using Domain.Commands;
using Interface.Resources;

namespace Interface.Assemblers;

public class SignUpConsumerCommandFromResourceAssembler
{
    public static SignUpConsumerCommand Assemble(SignUpConsumerResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        return new SignUpConsumerCommand
        {
            Email = resource.Email,
            Password = resource.Password,
            FirstName = resource.FirstName,
            LastName = resource.LastName
        };
    }
}
