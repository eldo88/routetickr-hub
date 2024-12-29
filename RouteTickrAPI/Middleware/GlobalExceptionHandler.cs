namespace RouteTickrAPI.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex switch
        {
            InvalidOperationException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var errorResponse = new
        {
            Message = ex.Message,
            StatusCode = context.Response.StatusCode,
            Timestamp = DateTime.UtcNow
        };

        return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(errorResponse));
    }
}