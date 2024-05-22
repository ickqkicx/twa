using Microsoft.EntityFrameworkCore;
using ControllerBasedApi.Models;

namespace ControllerBasedApi.Data;

public class TodoContext : DbContext
{
    public DbSet<TodoItem> Todos { get; set; }
    public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
}
