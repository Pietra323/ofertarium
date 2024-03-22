using backend.Data.Models;
using backend.Data.Models.DataBase;
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
    
    public async Task<IEnumerable<Product>> GetAllProducts(int userId)
    {
        var userProducts = await _ctx.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Products)
            .ToListAsync();
        return userProducts;
    }
    
    public async Task<Product> GetProductById(int userId, int productId)
    {
        var userProductById = await _ctx.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Products)
            .FirstOrDefaultAsync(p => p.Id == productId);
        return userProductById;
    }
    
    public async Task<Product> CreateProduct(int userId, Product product)
    {
        product.UserId = userId;
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