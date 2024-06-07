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
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<AccountSettings> AccountSettings { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Favourite> Favourities { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<OnSale> OnSales { get; set; }
    public DbSet<PaymentCard> Payments { get; set; }
    public DbSet<CategoryProduct> CategoryProducts { get; set; }
    public DbSet<BasketProduct> BasketProducts { get; set; }
    public DbSet<History> Histories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<UserFavourite> UserFavourites { get; set; }


    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        
        
        //relacje Usera jako model nadrzędny
        modelBuilder.Entity<User>()
            .HasMany(e => e.Products)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Photos)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);
        
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.AccountSettings)
            .WithOne(a => a.User)
            .HasForeignKey<AccountSettings>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        modelBuilder.Entity<User>()
            .HasMany(o => o.PaymentCards)
            .WithOne(u => u.User)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        //relacje AccountSettings jako model nadrzędny
        modelBuilder.Entity<AccountSettings>()
            .HasMany(o => o.Locations)
            .WithOne(u => u.AccountSettings)
            .HasForeignKey(o => o.AccountSettingsId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        

        //relacja BasketProduct(Basket,Product) - many to many
        modelBuilder.Entity<BasketProduct>()
            .HasKey(op => new { op.BasketId, op.ProductId });
        
        modelBuilder.Entity<BasketProduct>()
            .HasOne(op => op.Basket)
            .WithMany(o => o.BasketProducts)
            .HasForeignKey(op => op.BasketId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        modelBuilder.Entity<BasketProduct>()
            .HasOne(op => op.product)
            .WithMany(p => p.BasketProducts)
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        
        
        
        
        
        
        
        
        
        

        modelBuilder.Entity<User>()
            .HasOne(r => r.Basket)
            .WithOne(sr => sr.User)
            .HasForeignKey<Basket>(sr => sr.Id)
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
        
        modelBuilder.Entity<History>()
            .HasKey(h => new { h.OrderId, h.UserId, h.ProductId });

        modelBuilder.Entity<History>()
            .HasOne(h => h.User)
            .WithMany(u => u.Histories)
            .HasForeignKey(h => h.UserId);

        modelBuilder.Entity<History>()
            .HasOne(h => h.Product)
            .WithMany(p => p.Histories)
            .HasForeignKey(h => h.ProductId);
        
        //relacja OrderProduct(Product,Order) - many to many
        modelBuilder.Entity<UserFavourite>()
            .HasKey(uf => new { uf.UserId, uf.FavouriteId, uf.ProductId });

        modelBuilder.Entity<UserFavourite>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.UserFavourite)
            .HasForeignKey(uf => uf.UserId);

        modelBuilder.Entity<UserFavourite>()
            .HasOne(uf => uf.Favourite)
            .WithMany(f => f.UserFavourites)
            .HasForeignKey(uf => uf.FavouriteId);
        
        modelBuilder.Entity<UserFavourite>()
            .HasOne(uf => uf.Product)
            .WithMany(p => p.UserFavourites)
            .HasForeignKey(uf => uf.ProductId);
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies().UseMySQL(_ConnectionString);
    }
}