using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.Data.Models;

public class UserContext : DbContext
{
    public UserContext() { }
        
    private readonly IConfiguration _configuration;
    private readonly string _ConnectionString;

    public UserContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _ConnectionString = _configuration.GetConnectionString("default");
    }
    
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_ConnectionString);
    }
}