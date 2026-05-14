using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Klippr_Backend.IAM.Interface.Pipeline;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError($"An unhandled exception has occurred: {exception.Message}");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Message = exception.Message,
            ExceptionType = exception.GetType().Name
        };

        context.Response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string ExceptionType { get; set; } = string.Empty;
}
