using RouteTickrAPI.DTOs;

namespace RouteTickrAPI.Services;

public interface IAuthService
{
    Task<ServiceResult<string>> Login(LoginRequestDto request);
    Task<ServiceResult<UserDto>> Register(LoginRequestDto request);
}