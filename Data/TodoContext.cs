using Microsoft.EntityFrameworkCore;
using RestfulAPI.Data.Configurations;
using RestfulAPI.Data.Entities;

namespace RestfulAPI.Data;

public class TodoContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<TodoItem> Todos { get; set; }

    public TodoContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("TodoContext"));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

}