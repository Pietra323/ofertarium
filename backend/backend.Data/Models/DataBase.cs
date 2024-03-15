using backend.Data.Models.ManyToManyConnections;
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
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<AccountSettings> AccountSettings { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Bucket> Buckets { get; set; }
    public DbSet<BuyerRate> BuyerRates { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Favourite> Favourities { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<OnSale> OnSales { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<SellerRate> SellerRates { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<WishList> WishLists { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //relacja User,Product - one to many
        modelBuilder.Entity<User>()
            .HasMany(e => e.Products)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        //relacja User,Location - one to many
        modelBuilder.Entity<Location>()
            .HasOne(o => o.User)
            .WithMany(u => u.Locations)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        //relacja Order,User - one to many
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .IsRequired();
        
        //relacja OrderProduct(Product,Order) - many to many
        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderId, op.ProductId });
        
        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_ConnectionString);
    }
}