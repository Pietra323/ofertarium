using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.Data.Models;

public class DataBase : DbContext
{
    public DataBase() { }
        
    private readonly IConfiguration _configuration;
    private readonly string _ConnectionString;

    public DataBase(IConfiguration configuration)
    {
        _configuration = configuration;
        _ConnectionString = _configuration.GetConnectionString("default");
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_ConnectionString);
    }
}