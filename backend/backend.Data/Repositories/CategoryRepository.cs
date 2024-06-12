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

    public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
    {
        var categories = await _ctx.Categories.ToListAsync();
        var categoryDTO = categories.Select(categories => new CategoryDTO
        {
            Id = categories.Id,
            Nazwa = categories.Nazwa,
            Description = categories.Description
        });
        return categoryDTO;
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


    public async Task<CategoryDTO> CreateCategory(Category kategoria)
    {
        _ctx.Categories.Add(kategoria);
        var categoryDTO = new CategoryDTO()
        {
            Id = kategoria.Id,
            Nazwa = kategoria.Nazwa,
            Description = kategoria.Description,
            
        };
        await _ctx.SaveChangesAsync();
        return categoryDTO;
    }

    public async Task DeleteCategory(int id)
    {
        var category = await _ctx.Categories.FindAsync(id);
        _ctx.Categories.Remove(category);
        await _ctx.SaveChangesAsync();
    }

    public async Task AddCategoryProducts(int productIdProduct, List<int> productCategoryIds)
    {
        foreach (var productCategoryId in productCategoryIds)
        {
            var CategoryProduct = new CategoryProduct()
            {
                ProductId = productIdProduct,
                CategoryId = productCategoryId
            };
            await _ctx.CategoryProducts.AddAsync(CategoryProduct);
        }
        await _ctx.SaveChangesAsync();
    }
    
    public async Task<Category> GetCategoryByName(string name)
    {
        return await _ctx.Categories.FirstOrDefaultAsync(c => c.Nazwa == name);
    }

}