using backend.Data.Models;
using backend.Data.Models.DataBase;
using backend.Data.Models.ManyToManyConnections;
using backend.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataBase _ctx;
    
    public ProductRepository(DataBase ctx)
    {
        _ctx = ctx;
    }
    
    public async Task AddProductAsync(Product product)
    {
        _ctx.Products.Add(product);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<Product>> GetAllUserProducts(int userId)
    {
        var userProducts = await _ctx.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Products)
            .ToListAsync();
        return userProducts;
    }
    
    public async Task<IEnumerable<ProductDTO>> GetAllProducts()
    {
        var products = await _ctx.Products
            .Include(p => p.CategoryProducts)
            .ThenInclude(cp => cp.Category)
            .ToListAsync();

        var productDTOs = new List<ProductDTO>();

        foreach (var product in products)
        {
            var categoryIds = new List<int>();

            if (product.CategoryIds != null)
            {
                foreach (var categoryProduct in product.CategoryIds)
                {
                    categoryIds.Add(categoryProduct);
                }
            }

            var productDTO = new ProductDTO()
            {
                IdProduct = product.IdProduct,
                ProductName = product.ProductName,
                Subtitle = product.Subtitle,
                amountOf = product.amountOf,
                Price = product.Price,
                CategoryIds = categoryIds
            };

            productDTOs.Add(productDTO);
        }

        return productDTOs;
    }

    
    public async Task<IEnumerable<Product>> GetAllProductsByCategory(int category)
    {
        var productsInCategory = await _ctx.Products
            .Where(p => p.CategoryProducts.Any(cp => cp.Category.Id == category))
            .ToListAsync();

        return productsInCategory;
    }

    public async Task<Product?> GetProductById(int productId)
    {
        return await _ctx.Products.FindAsync(productId);
    }
    
    public async Task<ProductDTO?> GetProductByIdDTO(int productId)
    {
        var product = await _ctx.Products.FindAsync(productId);
        if (product == null) return null;

        var productDTO = new ProductDTO
        {
            IdProduct = product.IdProduct,
            ProductName = product.ProductName,
            Subtitle = product.Subtitle,
            amountOf = product.amountOf,
            Price = product.Price,
            CategoryIds = product.CategoryProducts
                .Where(cp => cp.CategoryId.HasValue)
                .Select(cp => cp.CategoryId.Value)
                .ToList()
        };
        return productDTO;
    }
    
    public async Task<Product> CreateProduct(int userId, Product product)
    {
        product.UserId = userId;
        await _ctx.Products.AddAsync(product);
        await _ctx.SaveChangesAsync();
        return product;
    }
    
    public async Task UpdateProduct(Product product)
    {
        _ctx.Products.Update(product);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task DeleteProduct(Product product)
    {
        _ctx.Products.Remove(product);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<ProductDTO>> GetUserFavouritesByUserIdAsyncNOW(int userId)
    {
        var favourites = await _ctx.UserFavourites
            .Where(uf => uf.UserId == userId)
            .Include(uf => uf.Product)
            .ThenInclude(p => p.CategoryProducts)
            .ToListAsync();

        var productDTOs = new List<ProductDTO>();
            
        foreach (var favourite in favourites)
        {
            var categoryIds = new List<int>();

            if (favourite.Product.CategoryProducts != null)
            {
                foreach (var categoryProduct in favourite.Product.CategoryIds)
                {
                    categoryIds.Add(categoryProduct);
                }
            }

            var productDTO = new ProductDTO()
            {
                IdProduct = favourite.ProductId,
                ProductName = favourite.Product.ProductName,
                Subtitle = favourite.Product.Subtitle,
                amountOf = favourite.Product.amountOf,
                Price = favourite.Product.Price,
                CategoryIds = categoryIds
            };

            productDTOs.Add(productDTO);
        }

        return productDTOs;
    }

    
    public async Task AddUserFavouriteAsync(UserFavourite userFavourite)
    {
        _ctx.UserFavourites.Add(userFavourite);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task<Favourite?> GetFavouriteByUserIdAndProductIdAsync(int userId, int productId)
    {
        return await _ctx.Favourities
            .Where(f => f.UserFavourites.Any(uf => uf.UserId == userId && uf.ProductId == productId))
            .FirstOrDefaultAsync();
    }
    
    public async Task RestoreOldPriceAndRemoveOnSale(int productId)
    {
        var onSale = await _ctx.OnSales.FirstOrDefaultAsync(os => os.ProductId == productId);
        if (onSale != null)
        {
            var product = await _ctx.Products.FindAsync(productId);
            if (product != null)
            {
                product.Price = onSale.OldPrice;
                _ctx.Products.Update(product);
            }
            _ctx.OnSales.Remove(onSale);
            await _ctx.SaveChangesAsync();
        }
    }

    
    public async Task CreateFavouriteAsync(Favourite favourite)
    {
        _ctx.Favourities.Add(favourite);
        await _ctx.SaveChangesAsync();
    }

    public async Task AddOnSale(int days, int months, int hours, int minutes, decimal newPrice, int productId)
    {
        var existingProduct = await _ctx.Products.FindAsync(productId);
        if (existingProduct == null)
        {
            throw new NullReferenceException("Product not found in the database.");
        }

        var onSale = new OnSale
        {
            OldPrice = existingProduct.Price,
            OnSalePrice = newPrice,
            ProductId = existingProduct.IdProduct,
            ExpirationTime = DateTime.Now.AddDays(days).AddMonths(months).AddHours(hours).AddMinutes(minutes)
        };
        existingProduct.Price = newPrice;
        _ctx.OnSales.Add(onSale);
        _ctx.Products.Update(existingProduct);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task<OnSale?> GetOnSaleByProductId(int productId)
    {
        return await _ctx.OnSales
            .FirstOrDefaultAsync(os => os.ProductId == productId);
    }
}
