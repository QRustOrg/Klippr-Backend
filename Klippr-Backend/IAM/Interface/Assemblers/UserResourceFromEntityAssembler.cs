using Domain.Aggregates;
using Interface.Resources;

namespace Interface.Assemblers;

public class UserResourceFromEntityAssembler
{
    public static UserResource Assemble(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return new UserResource
        {
            UserId = user.Id,
            Email = user.Email.Value,
            Role = user.Role.Value,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BusinessName = user.BusinessName,
            TaxId = user.TaxId,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public static IEnumerable<UserResource> AssembleMany(IEnumerable<User> users)
    {
        if (users == null)
            throw new ArgumentNullException(nameof(users));

        return users.Select(Assemble);
    }
}
