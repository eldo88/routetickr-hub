namespace RouteTickrAPI.DTOs;
//DTO that is return to client
public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Role { get; set; } = "";
}