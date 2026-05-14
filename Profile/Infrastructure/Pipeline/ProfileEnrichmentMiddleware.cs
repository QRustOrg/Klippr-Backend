using Microsoft.AspNetCore.Http;

namespace Klippr_Backend.Profile.Infrastructure.Pipeline;

public class ProfileEnrichmentMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ProfileEnrichmentMiddleware> _logger;

    public ProfileEnrichmentMiddleware(RequestDelegate next, ILogger<ProfileEnrichmentMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.LogInformation($"Profile Request: {context.Request.Method} {context.Request.Path}");

            // Enrich request with additional context
            var userId = context.User.FindFirst("sub")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                context.Items["UserId"] = userId;
            }

            await _next(context);

            // Enrich response with additional headers
            context.Response.Headers.Add("X-Processing-Time", DateTime.UtcNow.Ticks.ToString());
            _logger.LogInformation($"Profile Response: {context.Response.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Profile enrichment error: {ex.Message}");
            throw;
        }
    }
}
