using Microsoft.EntityFrameworkCore;
using MinimalApi.Models;

namespace MinimalApi.Data;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public DbSet<TodoItem> Todos { get; set; }

}
