using Account.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Account.Data;

public class AuthDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<GoogleUser> GoogleUsers { get; set; } = default!;


    public AuthDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString(nameof(AuthDbContext)));
        base.OnConfiguring(optionsBuilder);
    }
}
