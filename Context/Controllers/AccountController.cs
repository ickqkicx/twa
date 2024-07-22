using Account.Data;
using Account.Data.Entities;
using Account.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Account.Controllers;

[ApiController]
[Route("Account")]
public class AccountController(AuthDbContext dbContext, TokenService tokenService, EmailService emailService) : ControllerBase
{
    private readonly AuthDbContext _dbContext = dbContext;
    private readonly TokenService _tokenService = tokenService;
    private readonly EmailService _emailService = emailService;

    [AllowAnonymous]
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpGoogleUser signup)
    {
        var user = await _dbContext.GoogleUsers.FirstOrDefaultAsync(u => u.Email == signup.Email);
        if (user != null) return BadRequest("A user with this email address already exists.");

        user = signup;
        _dbContext.GoogleUsers.Add(user);
        await _dbContext.SaveChangesAsync();

        var token = _tokenService.GenerateConfirmationToken();
        var callbackUrl = Url.Action("Confirm", "auth", new { v1 = user.Id, v2 = token }, HttpContext.Request.Scheme);
        var body = $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>";
        await _emailService.SendAsync(user.Email, EmailService.ConfirmSubject, body);

        return Ok("Check your email, open and click by link.");
    }

    [AllowAnonymous]
    [HttpGet("Confirm")]
    public async Task<IActionResult> Confirm(string v1, string v2)
    {
        var isValid = _tokenService.IsValidConfirmationToken(v2);
        if (isValid == false) return BadRequest();

        var user = await _dbContext.GoogleUsers.FindAsync(v1);
        if (user == null) return BadRequest();
        user.IsConfirmed = true;
        _dbContext.Entry(user).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_dbContext.GoogleUsers.Any(e => e.Id == v1) == false) return NotFound();
            throw;
        }

        return Ok("Account confirmed.");
    }


    [AllowAnonymous]
    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordGoogleUser forgotPassword)
    {
        var user = await _dbContext.GoogleUsers.FirstOrDefaultAsync(u => u.Email == forgotPassword.Email);
        if (user == null) return BadRequest("User does not exist.");

        var token = _tokenService.GeneratePasswordResetToken();
        var callbackUrl = Url.Action("PasswordReset", "auth", new { v1 = user.Id, v2 = token }, HttpContext.Request.Scheme);
        var body = $"Для восстановления пароля перейдите по ссылке: <a href='{callbackUrl}'>link</a>";
        await _emailService.SendAsync(user.Email, EmailService.PasswordResetSubject, body);

        return Ok("Check your email, open and click by link.");
    }

    [AllowAnonymous]
    [HttpPost("PasswordReset")]
    public async Task<IActionResult> PasswordReset(string v1, string v2, [FromBody] PasswordResetGoogleUser passwordReset)
    {
        var isValid = _tokenService.IsValidPasswordResetToken(v2);
        if (isValid == false) return BadRequest();

        var user = await _dbContext.GoogleUsers.FindAsync(v1);
        if (user == null) return BadRequest();
        user.Password = passwordReset.Password;
        _dbContext.Entry(user).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_dbContext.GoogleUsers.Any(e => e.Id == v1) == false) return NotFound();
            throw;
        }

        return Ok("Password reseted.");
    }

}
