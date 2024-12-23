using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers;


[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenRepository _tokenRepository;
    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
       
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register ([FromBody] RegisterRequestDto registerRequestDto) {

        var IdentityUser = new IdentityUser {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };

        var IdentityResult = await _userManager.CreateAsync(IdentityUser, registerRequestDto.Password);

        if (IdentityResult.Succeeded) 
        {
            if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any()) 
            {
                IdentityResult = await _userManager.AddToRolesAsync(IdentityUser, registerRequestDto.Roles);
                if(IdentityResult.Succeeded)
                {
                    return Ok("User created successfully");
                }
            }
        }
        return BadRequest("User Registration Failed");
        
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login ([FromBody] LoginRequestDto loginRequestDto) {
        var IdentityUser = await _userManager.FindByEmailAsync(loginRequestDto.Username);
        if ( IdentityUser == null ) 
        {
            return BadRequest("Invalid Username or Password");
        }

        var IdentityResult = await _userManager.CheckPasswordAsync(IdentityUser, loginRequestDto.Password);
        if ( IdentityResult ) 
        {
            var roles = await _userManager.GetRolesAsync(IdentityUser);
            if(roles != null ) 
            {
                var jwtToken = _tokenRepository.CreateJWTToken(IdentityUser, roles.ToArray());
                var response = new TokenDto {
                    Token= jwtToken

                };
                return Ok(response);
            }
        }
        return BadRequest("Ivalid Username or Password");
    }
}

