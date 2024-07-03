using Microsoft.EntityFrameworkCore;
using Auth.Models;

namespace Auth.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Role> Roles { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(e => e.Roles)
            .WithMany(e => e.Users)
            .UsingEntity(e => e.ToTable("UserRole"));
    }
}
