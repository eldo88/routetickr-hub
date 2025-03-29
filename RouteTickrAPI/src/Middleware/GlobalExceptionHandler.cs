using System.Diagnostics;
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

        var statusCode = SetStatusCode(e);

        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new
        {
            Message = e.Message,
            StatusCode = context.Response.StatusCode,
            Timestamp = DateTime.UtcNow
        };


        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }

    private static HttpStatusCode SetStatusCode(Exception e)
    {
        return e switch
        {
            ArgumentNullException => HttpStatusCode.InternalServerError,
            ArgumentException => HttpStatusCode.InternalServerError,
            InvalidOperationException => HttpStatusCode.InternalServerError,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            DbUpdateException => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.InternalServerError
        };
    }
}