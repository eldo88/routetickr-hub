using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace RouteTickrAPI.Middleware;

public class GlobalExceptionHandler(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception e)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, errorMessage) = e switch
        {
            ArgumentException => (HttpStatusCode.BadRequest, "Invalid request."),
            InvalidOperationException => (HttpStatusCode.NotFound, "Resource not found."),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access."),
            DbUpdateException => (HttpStatusCode.InternalServerError, "Database error occurred."),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new
        {
            Message = errorMessage,
            StatusCode = (int)statusCode,
            Timestamp = DateTime.UtcNow
        };


        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}