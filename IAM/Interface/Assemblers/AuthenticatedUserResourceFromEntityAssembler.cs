using Domain.Aggregates;
using Interface.Resources;

namespace Interface.Assemblers;

public class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource Assemble(User user, string token)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be null or empty.", nameof(token));

        return new AuthenticatedUserResource
        {
            UserId = user.Id,
            Email = user.Email.Value,
            Role = user.Role.Value,
            Token = token
        };
    }
}
