using backend.Data.Models;
using backend.Data.Models.ManyToManyConnections;
using Microsoft.AspNetCore.Http;

namespace backend.Data.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<ProductDTO>> GetAllUserProducts(int userId);
    Task<IEnumerable<ProductDTO>> GetAllProducts();
    Task AddUserFavouriteAsync(UserFavourite userFavourite);
    Task<Favourite?> GetFavouriteByUserIdAndProductIdAsync(int userId, int productId);
    Task CreateFavouriteAsync(Favourite favourite);

    Task<Product> CreateProduct(int userId, ProductDTO productDTO, List<IFormFile> photos, string description);
    Task<Product> SEEDCreateProduct(int userId, ProductDTO productDTO);

    Task UpdateProduct(Product product);
    Task DeleteProduct(ProductDTO product);
    Task<IEnumerable<ProductDTO>> GetAllProductsByCategoryAsync(int categoryId);

    Task<IEnumerable<ProductDTO>> GetUserFavouritesByUserIdAsyncNOW(int userId);
    Task<ProductDTO?> GetProductById(int productId);
    Task RestoreOldPriceAndRemoveOnSale(int productId);
    Task AddOnSale(int days, int months, int hours, int minutes, decimal newPrice, int productId);
    Task<OnSale?> GetOnSaleByProductId(int productId);
    Task RemoveUserFavouriteAsync(int userId, int productId);
    Task AddPhotoToProduct(int productId, IFormFile file, string description);
}