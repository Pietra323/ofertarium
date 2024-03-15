using backend.Data.Models;

namespace backend.Data.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts(int userId);
    Task<Product> GetProductById(int userId, int productId);
    Task<Product> CreateProduct(int userId, Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(Product product);
}