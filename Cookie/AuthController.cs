using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        var user = _dbContext.Users
            .Include(u => u.Roles)
            .FirstOrDefault(u => u.Login == signin.Login && u.Password == signin.Password);

        if (user is null)
            return NotFound("Пользователь не найден!");

        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Login),
        };
        var claimsRoles = user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Name));
        claims.AddRange(claimsRoles);

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        return Ok();
    }

    [Authorize]
    [HttpGet("SignOut")]
    public async Task<ActionResult> SignOutAuth()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

}
