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
    public DbSet<Rate> Rates { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //relacja User,Product - one to many
        modelBuilder.Entity<User>()
            .HasMany(e => e.Products)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Konfiguracja relacji między Rate a BuyerRate
        modelBuilder.Entity<AccountSettings>()
            .HasOne(r => r.User)
            .WithOne(br => br.AccountSettings)
            .HasForeignKey<User>(br => br.Id);
        
        // Konfiguracja relacji między Rate a BuyerRate
        modelBuilder.Entity<Rate>()
            .HasOne(r => r.BuyerRate)
            .WithOne(br => br.Rate)
            .HasForeignKey<BuyerRate>(br => br.Id);

        // Konfiguracja relacji między Rate a SellerRate
        modelBuilder.Entity<Rate>()
            .HasOne(r => r.SellerRate)
            .WithOne(sr => sr.Rate)
            .HasForeignKey<SellerRate>(sr => sr.Id);
        
        //costam
        modelBuilder.Entity<Order>()
            .HasOne(r => r.Receipt)
            .WithOne(sr => sr.Order)
            .HasForeignKey<Receipt>(sr => sr.Id);
        
        //relacja AccountSettings,Location - one to many
        modelBuilder.Entity<Location>()
            .HasOne(o => o.AccountSettings)
            .WithMany(u => u.Locations)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        modelBuilder.Entity<Delivery>()
            .HasOne(o => o.User)
            .WithMany(u => u.Deliveries)
            .HasForeignKey(o => o.UserId)
            .IsRequired();
        
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
        
        //relacja OrderProduct(Product,Order) - many to many
        modelBuilder.Entity<CategoryProduct>()
            .HasKey(op => new { op.CategoryId, op.ProductId });
        
        modelBuilder.Entity<CategoryProduct>()
            .HasOne(op => op.Category)
            .WithMany(o => o.CategoryProducts)
            .HasForeignKey(op => op.CategoryId);

        modelBuilder.Entity<CategoryProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.CategoryProducts)
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_ConnectionString);
    }
}