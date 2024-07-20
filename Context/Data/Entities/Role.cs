using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Data.Entities;

[Table("Role", Schema = "dbo")]
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; } = [];
}
