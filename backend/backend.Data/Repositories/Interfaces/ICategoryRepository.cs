using System.Collections;
using backend.Data.Models;

namespace backend.Data.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategories();
    Task CreateCategory(string categoryName, string description);
    Task DeleteCategory(int id);
}