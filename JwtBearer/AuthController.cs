using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Auth.Data;
using Auth.Models;

namespace Auth.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(AuthDbContext dbContext) : ControllerBase
{
    private readonly AuthDbContext _dbContext = dbContext;

    [AllowAnonymous]
    [HttpPost("SignIn")]
    public async Task<ActionResult> SignIn(SignInUser signin)
    {
        var user = await _dbContext.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Login == signin.Login && u.Password == signin.Password);

        if (user is null)
            return NotFound("Пользователь не найден!");

        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Login),
        };
        var claimsRoles = user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Name));
        claims.AddRange(claimsRoles);

        var jwt = new JwtSecurityToken(
            issuer: "server",
            audience: "client",
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwertyuiopasdfghjklzxcvbnm123456")),
                SecurityAlgorithms.HmacSha256));

        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }
}
