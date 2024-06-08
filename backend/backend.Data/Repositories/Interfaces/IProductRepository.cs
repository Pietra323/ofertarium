using backend.Data.Models;
using backend.Data.Models.ManyToManyConnections;

namespace backend.Data.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<ProductDTO>> GetAllUserProducts(int userId);
    Task<IEnumerable<ProductDTO>> GetAllProducts();
    Task AddProductAsync(Product product);
    Task AddUserFavouriteAsync(UserFavourite userFavourite);
    Task<Favourite?> GetFavouriteByUserIdAndProductIdAsync(int userId, int productId);
    Task<IEnumerable<Product>> GetAllProductsByCategory(int category);
    Task CreateFavouriteAsync(Favourite favourite);

    Task<Product> CreateProduct(int userId, ProductDTO product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(ProductDTO product);
    Task<IEnumerable<ProductDTO>> GetUserFavouritesByUserIdAsyncNOW(int userId);
    Task<ProductDTO?> GetProductById(int productId);
    Task RestoreOldPriceAndRemoveOnSale(int productId);
    Task AddOnSale(int days, int months, int hours, int minutes, decimal newPrice, int productId);
    Task<OnSale?> GetOnSaleByProductId(int productId);
    Task RemoveUserFavouriteAsync(int userId, int productId);
}