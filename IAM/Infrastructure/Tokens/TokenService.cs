using Klippr_Backend.IAM.Application.OutboundServices.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Klippr_Backend.IAM.Infrastructure.Tokens;

public class TokenService : ITokenService
{
    private readonly string _secretKey;
    private readonly int _expirationMinutes;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(string secretKey, int expirationMinutes = 60, string issuer = "klippr-iam", string audience = "klippr-api")
    {
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new ArgumentException("Secret key cannot be null or empty.", nameof(secretKey));

        if (secretKey.Length < 32)
            throw new ArgumentException("Secret key must be at least 32 characters long.", nameof(secretKey));

        if (expirationMinutes < 1)
            throw new ArgumentException("Expiration minutes must be greater than 0.", nameof(expirationMinutes));

        _secretKey = secretKey;
        _expirationMinutes = expirationMinutes;
        _issuer = issuer;
        _audience = audience;
    }

    public string GenerateToken(Guid userId, string email, string role)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role cannot be null or empty.", nameof(role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (Guid UserId, string Email, string Role) ValidateAndExtractClaims(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be null or empty.", nameof(token));

        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var handler = new JwtSecurityTokenHandler();

            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;
            var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || string.IsNullOrWhiteSpace(emailClaim) || string.IsNullOrWhiteSpace(roleClaim))
                throw new SecurityTokenException("Token claims are invalid.");

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new SecurityTokenException("Invalid user ID in token.");

            return (userId, emailClaim, roleClaim);
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException("Token validation failed.", ex);
        }
    }
}
