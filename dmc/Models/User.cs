using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Models;

[Table("User", Schema = "dbo")]
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }  
    public string Password { get; set; }

    public List<Role> Roles { get; set; } = [];
}


public class SingInUser
{
    public string Login { get; set; }
    public string Password { get; set; }
}