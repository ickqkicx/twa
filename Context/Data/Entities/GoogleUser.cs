using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Account.Data.Entities;

[Table("GoogleUser", Schema = "dbo")]
public class GoogleUser
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string FullName => $"{Name} {Surname}";
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsConfirmed { get; set; }

    public static GoogleUser CreateGoogleUserFromClaims(ClaimsPrincipal claims)
    {
        var id = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
        var name = claims.FindFirst(ClaimTypes.GivenName).Value;
        var surname = claims.FindFirst(ClaimTypes.Surname).Value;
        var email = claims.FindFirst(ClaimTypes.Email).Value;
        var googleuser = new GoogleUser() { Id = id, Name = name, Surname = surname,
            Email = email, Password = Guid.NewGuid().ToString() };

        return googleuser;
    }

    public static implicit operator GoogleUser(SignUpGoogleUser signup)
    {
        var user = new GoogleUser() { Id = Guid.NewGuid().ToString(),
            Name = signup.Name, Surname = signup.Surname,
            Email = signup.Email, Password = signup.Password, IsConfirmed = false };
        return user;
    }
}

public class SignInGoogleUser
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}

public class SignUpGoogleUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}

public class ForgotPasswordGoogleUser
{
    [EmailAddress]
    public string Email { get; set; }
}

public class PasswordResetGoogleUser
{
    public string Password { get; set; }
}
