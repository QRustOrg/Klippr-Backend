using Application.OutboundServices.Tokens;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Pipeline;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenService _tokenService;

    public AuthorizationMiddleware(RequestDelegate next, ITokenService tokenService)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authorizationHeader = context.Request.Headers.Authorization.ToString();

        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            try
            {
                var token = authorizationHeader.StartsWith("Bearer ")
                    ? authorizationHeader.Substring("Bearer ".Length).Trim()
                    : authorizationHeader;

                var (userId, email, role) = _tokenService.ValidateAndExtractClaims(token);
                context.Items["UserId"] = userId;
                context.Items["Email"] = email;
                context.Items["Role"] = role;
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid token.");
                return;
            }
        }

        await _next(context);
    }
}
