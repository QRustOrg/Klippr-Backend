namespace Klippr_Backend.IAM.Application.OutboundServices.Tokens;

public interface ITokenService
{
    string GenerateToken(Guid userId, string email, string role);
    (Guid UserId, string Email, string Role) ValidateAndExtractClaims(string token);
}
