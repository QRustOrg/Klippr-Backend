using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace Klippr_Backend.Profile.Infrastructure.Pipeline;

public class LocationCachingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LocationCachingMiddleware> _logger;
    private static readonly ConcurrentDictionary<string, object> LocationCache = new();
    private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(60);

    public LocationCachingMiddleware(RequestDelegate next, ILogger<LocationCachingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Check if this is a location-related request
            if (context.Request.Path.StartsWithSegments("/api/profiles/locations"))
            {
                var cacheKey = $"{context.Request.Method}:{context.Request.Path}{context.Request.QueryString}";

                if (LocationCache.TryGetValue(cacheKey, out var cachedValue))
                {
                    _logger.LogInformation($"Location cache hit: {cacheKey}");
                    context.Items["CachedLocation"] = cachedValue;
                }
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Location caching error: {ex.Message}");
            throw;
        }
    }
}
