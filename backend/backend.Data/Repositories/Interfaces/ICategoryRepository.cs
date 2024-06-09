using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Data.Models;

namespace backend.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO> CreateCategory(Category kategoria);
        Task DeleteCategory(int id);
        Task AddCategoryProducts(int productIdProduct, List<int> productCategoryIds);
        Task<Category> GetCategoryByName(string name);
    }
}