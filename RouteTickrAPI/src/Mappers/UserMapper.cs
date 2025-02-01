using RouteTickrAPI.Builders;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Models;

namespace RouteTickrAPI.Mappers;

public static class UserMapper
{
    public static User ToUser(LoginRequestDto dto)
    {
        return new UserBuilder()
            .WithUsername(dto.Username)
            .WithPassword(dto.Password)
            .WithRole()
            .Build();
    }

    public static User ToUser(UpdateUserDto dto)
    {
        return new UserBuilder()
            .WithId(dto.Id)
            .WithUsername(dto.Username)
            .WithRole()
            .Build();
    }

    public static User ToUser(ChangePasswordDto dto)
    {
        return new UserBuilder()
            .WithId(dto.Id)
            .WithPasswordChange(dto.NewPassword, dto.CurrentPassword)
            .Build();
    }

    public static UserDto ToUserDto(User user)
    {
        return new UserDto()
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        };
    }
}