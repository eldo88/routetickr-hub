using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Mappers;
using RouteTickrAPI.Repositories;

namespace RouteTickrAPI.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    public async Task<ServiceResult<string>> Login(LoginRequestDto request)
    {
        try
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return ServiceResult<string>.ErrorResult("User Unauthorized");
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var successToken = new JwtSecurityTokenHandler().WriteToken(token);
        
            return ServiceResult<string>.SuccessResult(successToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured in Login {ex.Message}");
            return ServiceResult<string>.ErrorResult("Unexpected error occured.");
        }
    }

    public async Task<ServiceResult<UserDto>> Register(LoginRequestDto request)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (existingUser is not null)
                return ServiceResult<UserDto>.ErrorResult("Username already exists");

            var newUser = UserMapper.ToUser(request);
            var isAdded = await _userRepository.AddUserAsync(newUser);

            if (!isAdded) return ServiceResult<UserDto>.ErrorResult("Error Registering User");
            var addedUser = UserMapper.ToUserDto(newUser);
        
            return ServiceResult<UserDto>.SuccessResult(addedUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured in Register {ex.Message}");
            return ServiceResult<UserDto>.ErrorResult("Unexpected error occured.");
        }
    }
}