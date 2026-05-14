using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Interface.Resources;

namespace Klippr_Backend.IAM.Interface.Assemblers;

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
