using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using Auth.Data;
using Auth.Data.Entities;

namespace Auth.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(AuthDbContext dbContext) : ControllerBase
{
    private readonly AuthDbContext _dbContext = dbContext;

    [AllowAnonymous]
    [HttpGet("SignInGoogle")]
    public async Task SignInGoogle()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleResponse)),

        });

        //var properties = new AuthenticationProperties() { RedirectUri = Url.Action(nameof(GoogleResponse)) };
        //return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [AllowAnonymous]
    [HttpGet("GoogleResponse")]
    public async Task<ActionResult> GoogleResponse()
    {
        var authenticate = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (authenticate.Succeeded == false ||
            authenticate.Principal == null)
            return BadRequest();

        var newGoogleUser = GoogleUser.CreateGoogleUserFromClaims(authenticate.Principal);

        var googleUser = await _dbContext.GoogleUsers.FindAsync(newGoogleUser.Id);
        if (googleUser == null)
        {
            _dbContext.GoogleUsers.Add(newGoogleUser);
            await _dbContext.SaveChangesAsync();
        }

        return Ok(JsonSerializer.Serialize(newGoogleUser));
    }

    [AllowAnonymous]
    [HttpPost("SignIn")]
    public async Task<ActionResult> SignIn(SignInGoogleUser signin)
    {
        var user = await _dbContext.GoogleUsers
            .FirstOrDefaultAsync(u => u.Email == signin.Email && u.Password == signin.Password);

        if (user is null)
            return NotFound("Пользователь не найден!");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.GivenName, user.Name),
            new(ClaimTypes.Surname, user.Surname),
            new(ClaimTypes.Name, user.FullName),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        return Ok();
    }

    [Authorize]
    [HttpGet("SignOut")]
    public async Task<ActionResult> SignOutAuth()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }

}
