using backend.Data.Models;
using backend.Data.Models.ManyToManyConnections;

namespace backend.Data.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllUserProducts(int userId);
    Task<IEnumerable<ProductDTO>> GetAllProducts();
    Task AddProductAsync(Product product);
    Task AddUserFavouriteAsync(UserFavourite userFavourite);
    Task<Favourite?> GetFavouriteByUserIdAndProductIdAsync(int userId, int productId);
    Task<IEnumerable<Product>> GetAllProductsByCategory(int category);
    Task CreateFavouriteAsync(Favourite favourite);

    Task<Product> CreateProduct(int userId, Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(Product product);
    Task<IEnumerable<ProductDTO>> GetUserFavouritesByUserIdAsyncNOW(int userId);
    Task<Product?> GetProductById(int productId);
    Task RestoreOldPriceAndRemoveOnSale(int productId);
    Task<ProductDTO?> GetProductByIdDTO(int productId);
    Task AddOnSale(int days, int months, int hours, int minutes, decimal newPrice, int productId);
    Task<OnSale?> GetOnSaleByProductId(int productId); 
}