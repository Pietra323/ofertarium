using backend.Data.Models;
using backend.Data.Models.DataBase;
using backend.Data.Models.ManyToManyConnections;
using backend.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataBase _ctx;

    public CategoryRepository(DataBase ctx)
    {
        _ctx = ctx;
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        var categories = await _ctx.Categories.ToListAsync();
        return categories;
    }
    
    public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
    {
        var productsInCategory = await _ctx.Categories
            .Where(c => c.Id == categoryId)
            .SelectMany(c => c.CategoryProducts)
            .Select(cp => cp.Product)
            .ToListAsync();
        
        return productsInCategory;
    }


    public async Task CreateCategory(string categoryName, string description)
    {
        var category = new Category()
        {
            Nazwa = categoryName,
            Description = description
        };
        _ctx.Categories.Add(category);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteCategory(int id)
    {
        var category = await _ctx.Categories.FindAsync(id);
        _ctx.Categories.Remove(category);
        await _ctx.SaveChangesAsync();
    }
}