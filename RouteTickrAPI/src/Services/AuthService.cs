using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Extensions;
using RouteTickrAPI.Repositories;

namespace RouteTickrAPI.Services;

public class AuthService(IUserRepository userRepository, IConfiguration config) : IAuthService
{
    public async Task<ServiceResult<string>> Login(LoginRequestDto request)
    {
        try
        {
            var user = await userRepository.GetUserByUsernameAsync(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return ServiceResult<string>.ErrorResult("User Unauthorized");
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? string.Empty));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
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
            var existingUser = await userRepository.GetUserByUsernameAsync(request.Username);
            if (existingUser is not null)
                return ServiceResult<UserDto>.ErrorResult("Username already exists");

            var newUser = request.ToUser();
            var isAdded = await userRepository.AddUserAsync(newUser);

            if (!isAdded) return ServiceResult<UserDto>.ErrorResult("Error Registering User");
            var addedUser = newUser.ToUserDto();
        
            return ServiceResult<UserDto>.SuccessResult(addedUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured in Register {ex.Message}");
            return ServiceResult<UserDto>.ErrorResult("Unexpected error occured.");
        }
    }
}