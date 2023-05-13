using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace ShedulingReminders.WebUI.Middlewares;

/// <summary>
/// Middleware for handling exceptions that occur during request processing.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger for logging exceptions.</param>
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to handle exceptions during request processing.
    /// </summary>
    /// <param name="httpContext">The current HTTP context.</param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (SqlException ex)
        {
            ErrorResult error = await HandleExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            ErrorResult error = await HandleExceptionAsync(httpContext, ex);
            _logger.LogError(ex, $"Request {httpContext.Request?.Method}: {httpContext.Request?.Path.Value} failed Error: {@error}", error);
        }
    }

    private async Task<ErrorResult> HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        ErrorResult response = new ErrorResult
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
        return response;
    }
}

