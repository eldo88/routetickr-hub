using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RouteTickrAPI.DTOs;
using RouteTickrAPI.Models;
using RouteTickrAPI.Repositories;
using RouteTickrAPI.Services;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace RouteTickrAPI.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public AuthController(IConfiguration config, IUserRepository userRepository, IAuthService authService)
    {
        _config = config;
        _userRepository = userRepository;
        _authService = authService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.Login(request);

        if (!result.Success) return Unauthorized(result);

        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginRequestDto request)
    {
        var result = await _authService.Register(request);
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }
}