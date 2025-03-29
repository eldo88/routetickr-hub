using System.Net;
using System.Text.Json;
using CsvHelper;
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
            ErrorMessage = e.Message,
            HttpStatusCode = context.Response.StatusCode,
            ErrorDetails = e.StackTrace,
            Timestamp = DateTime.UtcNow
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }

    private static HttpStatusCode SetStatusCode(Exception e)
    {
        return e switch
        {
            ArgumentNullException => HttpStatusCode.BadRequest,
            ArgumentException => HttpStatusCode.BadRequest,
            InvalidOperationException => HttpStatusCode.InternalServerError,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            DbUpdateException => HttpStatusCode.InternalServerError,
            CsvHelperException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
    }
}