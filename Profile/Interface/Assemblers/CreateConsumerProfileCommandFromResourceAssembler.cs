using Domain.Commands;
using Interface.Resources;

namespace Interface.Assemblers;

public class CreateConsumerProfileCommandFromResourceAssembler
{
    public static CreateConsumerProfileCommand ToCommand(ConsumerProfileResource resource, Guid userId)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        return new CreateConsumerProfileCommand
        {
            UserId = userId,
            FirstName = resource.FirstName,
            LastName = resource.LastName,
            PhoneNumber = resource.PhoneNumber
        };
    }
}
