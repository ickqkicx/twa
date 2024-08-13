using Microsoft.EntityFrameworkCore;
using GrpcTodoCore.Proto;
using GrpcTodoServer.Data.Configurations;

namespace GrpcTodoServer.Data;

public class TodoContext(IConfiguration configuration) : DbContext
{
    public DbSet<TodoItem> Todos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(configuration.GetConnectionString(nameof(TodoContext)));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

}