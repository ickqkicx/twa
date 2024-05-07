using Microsoft.EntityFrameworkCore;

namespace RestAPI.Models;

public class TodoContext : DbContext
{
    public DbSet<TodoItem> Todos { get; set; }
    public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
}
