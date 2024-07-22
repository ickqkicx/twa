using Auth.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data;

public class AuthDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<GoogleUser> GoogleUsers { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Role> Roles { get; set; } = default!;


    public AuthDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString(nameof(AuthDbContext)));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(e => e.Roles)
            .WithMany(e => e.Users)
            .UsingEntity(e => e.ToTable("UserRole"));
        base.OnModelCreating(modelBuilder);
    }
}
