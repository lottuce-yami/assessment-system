using AssessmentSystem.Data;
using AssessmentSystem.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ApplicationDbContext context, IConfiguration config, IPasswordHasher<User> passwordHasher) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly IConfiguration _config = config;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
        if (user == null)
        {
            return Unauthorized();
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMe()
    {
        return Ok(new { 
            id = User.FindFirstValue(ClaimTypes.NameIdentifier),
            username = User.FindFirstValue(ClaimTypes.Name) 
            });
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}