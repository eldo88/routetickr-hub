namespace RouteTickrAPI.Services;

public class ServiceResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }

    public static ServiceResult<T> SuccessResult(T data) => 
        new () { Success = true, Data = data };

    public static ServiceResult<T> ErrorResult(string errorMessage) =>
        new() { Success = false, ErrorMessage = errorMessage };
}