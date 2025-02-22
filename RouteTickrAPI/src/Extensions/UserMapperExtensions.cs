using RouteTickrAPI.Builders;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Extensions;

public static class UserMapperExtensions
{
    public static User ToUser(this LoginRequestDto dto)
    {
        return UserBuilder
            .Create()
            .WithUsername(dto.Username)
            .WithPassword(dto.Password)
            .WithRole()
            .Build();
    }

    public static User ToUser(this UpdateUserDto dto)
    {
        return UserBuilder
            .Create()
            .WithId(dto.Id)
            .WithUsername(dto.Username)
            .WithRole()
            .Build();
    }

    public static User ToUser(this ChangePasswordDto dto)
    {
        return UserBuilder
            .Create()
            .WithId(dto.Id)
            .WithPasswordChange(dto.NewPassword, dto.CurrentPassword)
            .Build();
    }

    public static UserDto ToUserDto(this User user)
    {
        return new UserDto()
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        };
    }
}