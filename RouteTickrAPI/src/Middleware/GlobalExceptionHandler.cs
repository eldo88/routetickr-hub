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
            ArgumentException => (HttpStatusCode.BadRequest, e.Message),
            InvalidOperationException => (HttpStatusCode.InternalServerError, e.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, e.Message),
            DbUpdateException => (HttpStatusCode.InternalServerError, e.Message),
            _ => (HttpStatusCode.InternalServerError, e.Message)
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