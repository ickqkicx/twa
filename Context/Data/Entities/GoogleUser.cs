using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Auth.Data.Entities;

[Table("GoogleUser", Schema = "dbo")]
public class GoogleUser
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string FullName => $"{Name} {Surname}";
    public string Email { get; set; }
    public string Password { get; set; }

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
}

public class SignInGoogleUser
{
    public string Email { get; set; }
    public string Password { get; set; }
}