using RouteTickr.Entities;

namespace RouteTickrAPI.Builders;

public class UserBuilder
{
    private readonly User _user = new ();

    public static UserBuilder Create() => new();

    public UserBuilder WithId(int id)
    {
        if (id < 0)
            throw new ArgumentException("Invalid Id");
        
        _user.Id = id;
        return this;
    }

    public UserBuilder WithUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty");

        _user.Username = username;
        return this;
    }

    public UserBuilder WithPassword(string plaintTextPassword)
    {
        if (string.IsNullOrWhiteSpace(plaintTextPassword))
            throw new ArgumentException("Password cannot be empty");

        _user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plaintTextPassword);
        return this;
    }
    
    public UserBuilder WithRole(string role = "User")
    {
        _user.Role = role;
        return this;
    }
    
    public User Build()
    {
        if (string.IsNullOrEmpty(_user.Username))
            throw new InvalidOperationException("Username is required.");
        
        if (string.IsNullOrEmpty(_user.PasswordHash))
            throw new InvalidOperationException("Password must be set.");
        
        return _user;
    }
    
    public UserBuilder WithPasswordChange(string newPlainTextPassword, string currentPasswordHash)
    {
        if (!BCrypt.Net.BCrypt.Verify(newPlainTextPassword, currentPasswordHash))
            throw new UnauthorizedAccessException("Current password is incorrect.");
        
        _user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPlainTextPassword);
        return this;
    }
}