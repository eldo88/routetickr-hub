namespace RouteTickrAPI.Services;

public class ServiceResult<T>
{
    public bool Success { get; private init; }
    public T? Data { get; private init; }
    public string? ErrorMessage { get; private init; }
    public string? NotFoundMessage { get; private init; }

    public static ServiceResult<T> SuccessResult(T data) => 
        new () { Success = true, Data = data };

    public static ServiceResult<T> ErrorResult(string errorMessage) =>
        new() { Success = false, ErrorMessage = errorMessage };

    public static ServiceResult<T> NotFoundResult(string message) =>
        new() { Success = false, NotFoundMessage = message };
}