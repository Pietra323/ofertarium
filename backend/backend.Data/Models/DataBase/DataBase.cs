using backend.Data.Models.ManyToManyConnections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.Data.Models.DataBase;

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
    public DbSet<SellerRate> SellerRates { get; set; }
    public DbSet<Rate> Rates { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //relacje Usera jako model nadrzędny
        modelBuilder.Entity<User>()
            .HasMany(e => e.Products)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.AccountSettings)
            .WithOne(a => a.User)
            .HasForeignKey<AccountSettings>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        //relacje AccountSettings jako model nadrzędny
        modelBuilder.Entity<AccountSettings>()
            .HasMany(o => o.Locations)
            .WithOne(u => u.AccountSettings)
            .HasForeignKey(o => o.AccountSettingsId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        modelBuilder.Entity<AccountSettings>()
            .HasMany(o => o.Discounts)
            .WithOne(u => u.AccountSettings)
            .HasForeignKey(o => o.AccountSettingsId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        modelBuilder.Entity<AccountSettings>()
            .HasMany(o => o.Payments)
            .WithOne(u => u.AccountSettings)
            .HasForeignKey(o => o.AccountSettingsId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        
        

        // Konfiguracja relacji między Rate a SellerRate
        modelBuilder.Entity<Rate>()
            .HasOne(r => r.SellerRate)
            .WithOne(sr => sr.Rate)
            .HasForeignKey<Rate>(sr => sr.Id)
            .IsRequired(false);
        
        
        modelBuilder.Entity<Rate>()
            .HasOne(r => r.BuyerRate)
            .WithOne(br => br.Rate)
            .HasForeignKey<BuyerRate>(br => br.Id)
            .IsRequired(false);
        
        
        
        
        
        
        
        //costam
        modelBuilder.Entity<Order>()
            .HasOne(r => r.Receipt)
            .WithOne(sr => sr.Order)
            .HasForeignKey<Order>(sr => sr.Id)
            .IsRequired(false);
        
        //costam2
        modelBuilder.Entity<Product>()
            .HasOne(r => r.OnSale)
            .WithOne(sr => sr.Product)
            .HasForeignKey<Product>(sr => sr.Id)
            .IsRequired(false);
        
        //costam3
        modelBuilder.Entity<Bucket>()
            .HasOne(r => r.User)
            .WithOne(sr => sr.Bucket)
            .HasForeignKey<Bucket>(sr => sr.Id)
            .IsRequired(false);
        
        //costam32
        modelBuilder.Entity<Delivery>()
            .HasOne(r => r.Order)
            .WithOne(sr => sr.Delivery)
            .HasForeignKey<Delivery>(sr => sr.Id)
            .IsRequired(false);
        
        //costam
        modelBuilder.Entity<Auction>()
            .HasOne(r => r.Product)
            .WithOne(sr => sr.Auction)
            .HasForeignKey<Auction>(sr => sr.Id)
            .IsRequired(false);
        
        modelBuilder.Entity<Bucket>()
            .HasOne(r => r.User)
            .WithOne(sr => sr.Bucket)
            .HasForeignKey<Bucket>(sr => sr.Id)
            .IsRequired(false);
        
        //relacja OrderProduct(Product,Order) - many to many
        modelBuilder.Entity<AuctionUser>()
            .HasKey(op => new { op.AuctionId, op.UserId });
        
        modelBuilder.Entity<AuctionUser>()
            .HasOne(op => op.Auction)
            .WithMany(o => o.AuctionUsers)
            .HasForeignKey(op => op.AuctionId)
            .IsRequired(false);

        // Dodaj relację między Bucket a Product
        modelBuilder.Entity<Bucket>()
            .HasMany(b => b.Products)
            .WithOne(p => p.Bucket)
            .HasForeignKey(p => p.BucketId)
            .IsRequired(false);

        modelBuilder.Entity<AuctionUser>()
            .HasOne(op => op.User)
            .WithMany(p => p.AuctionUsers)
            .HasForeignKey(op => op.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        
        //relacja AccountSettings,Location - one to many
        modelBuilder.Entity<Product>()
            .HasOne(o => o.Bucket)
            .WithMany(u => u.Products)
            .HasForeignKey(o => o.BucketId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        
        //relacja Order,User - one to many
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .IsRequired(false);
        
        //relacja OrderProduct(Product,Order) - many to many
        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderId, op.ProductId });
        
        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId)
            .IsRequired(false);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        //relacja OrderProduct(Product,Order) - many to many
        modelBuilder.Entity<CategoryProduct>()
            .HasKey(op => new { op.CategoryId, op.ProductId });
        
        modelBuilder.Entity<CategoryProduct>()
            .HasOne(op => op.Category)
            .WithMany(o => o.CategoryProducts)
            .HasForeignKey(op => op.CategoryId)
            .IsRequired(false);

        modelBuilder.Entity<CategoryProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.CategoryProducts)
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        //relacja OrderProduct(Product,Order) - many to many
        modelBuilder.Entity<UserFavourite>()
            .HasKey(op => new { op.UserId, op.FavouriteId });
        
        modelBuilder.Entity<UserFavourite>()
            .HasOne(op => op.User)
            .WithMany(o => o.UserFavourite)
            .HasForeignKey(op => op.UserId)
            .IsRequired(false);

        modelBuilder.Entity<UserFavourite>()
            .HasOne(op => op.Favourite)
            .WithMany(p => p.UserFavourite)
            .HasForeignKey(op => op.FavouriteId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        
        
        
        
        
        //SEED
        modelBuilder.Entity<User>().HasData(
            new User() { Id = 1, Name = "Joanna", LastName = "Przybysz", Username = "JPrzybysz", Email = "JPrzybysz@mail.com", Password = "JPrzybysz123.", isAdmin = false},
            new User() { Id = 2, Name = "Anna", LastName = "Utna", Username = "AUtna", Email = "AUtna@mail.com", Password = "AUtna123.", isAdmin = true}
            );
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies().UseMySQL(_ConnectionString);
    }
}
