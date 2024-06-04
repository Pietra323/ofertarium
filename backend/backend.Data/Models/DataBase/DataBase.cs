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
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Favourite> Favourities { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<OnSale> OnSales { get; set; }
    public DbSet<PaymentCard> Payments { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<Zdjecie> Zdjecia { get; set; }
    public DbSet<BasketProduct> BasketProducts { get; set; }
    public DbSet<CategoryProduct> CategoryProducts { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Zdjecies)
            .WithOne(z => z.Product)
            .HasForeignKey(z => z.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        
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
        

        /*
        modelBuilder.Entity<Product>()
            .HasOne(r => r.OnSale)
            .WithOne(sr => sr.Product)
            .HasForeignKey<Product>(sr => sr.IdProduct)
            .IsRequired(false);
        */

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
        
        
        
        
        
        
        
        //costam
        modelBuilder.Entity<Order>()
            .HasOne(r => r.Receipt)
            .WithOne(sr => sr.Order)
            .HasForeignKey<Order>(sr => sr.Id)
            .IsRequired(false);
        

        
        //costam32
        modelBuilder.Entity<Delivery>()
            .HasOne(r => r.Order)
            .WithOne(sr => sr.Delivery)
            .HasForeignKey<Delivery>(sr => sr.Id)
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
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies().UseMySQL(_ConnectionString);
    }
}