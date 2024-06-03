using backend.Data.Models;

namespace backend.Data.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllUserProducts(int userId);
    Task<IEnumerable<Product>> GetAllProducts();
    Task AddProductAsync(Product product);
    Task<IEnumerable<Product>> GetAllProductsByCategory(int category);

    Task<Product> GetProductById(int productId);
    Task<Product> CreateProduct(int userId, Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(Product product);
}