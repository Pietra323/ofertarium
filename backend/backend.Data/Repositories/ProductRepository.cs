using System.Data.Common;
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
    
    public async Task<IEnumerable<Product>> GetAllUserProducts(int userId)
    {
        var userProducts = await _ctx.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Products)
            .ToListAsync();
        return userProducts;
    }
    
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        var products = await _ctx.Products.ToListAsync();
        return products;
    }

    
    public async Task<IEnumerable<Product>> GetAllProductsByCategory(int category)
    {
        var productsInCategory = await _ctx.Products
            .Where(p => p.CategoryProducts.Any(cp => cp.Category.Id == category))
            .ToListAsync();

        return productsInCategory;
    }

    
    public async Task<Product> GetProductById(int productId)
    {
        return await _ctx.Products.FindAsync(productId);
    }
    
    
    
    public async Task<Product> CreateProduct(int userId, Product product)
    {
        product.UserId = userId;
        var categoryIds = product.CategoryIds;
        
        foreach (var categoryId in categoryIds)
        {
            var category = await _ctx.Categories.FindAsync(categoryId);
            if (category != null)
            {
                var categoryProduct = new CategoryProduct()
                {
                    Product = product,
                    Category = category
                };
                _ctx.CategoryProducts.Add(categoryProduct);
            }
        }
        _ctx.Products.Add(product);
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
}