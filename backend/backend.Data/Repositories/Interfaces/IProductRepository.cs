using backend.Data.Models;

namespace backend.Data.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllUserProducts(int userId);
    Task<IEnumerable<Product>> GetAllProducts();
    Task<IEnumerable<Product>> GetAllProductsByCategory(string category);

    Task<Product> GetProductById(int userId, int productId);
    Task<Product> CreateProduct(int userId, Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(Product product);
}