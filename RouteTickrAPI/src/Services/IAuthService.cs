using RouteTickrAPI.DTOs;
using RouteTickrAPI.Models;

namespace RouteTickrAPI.Services;

public interface IAuthService
{
    Task<ServiceResult<string>> Login(LoginRequestDto request);
    Task<ServiceResult<User>> Register(LoginRequestDto request);
}