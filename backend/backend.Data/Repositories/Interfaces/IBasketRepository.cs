using backend.Data.Models;
using backend.Data.Models.ManyToManyConnections;

namespace backend.Data.Repositories.Interfaces;

public interface IBasketRepository
{
    Task<Basket> AddToBasket(int userId, int productId);
    Task<Basket> RemoveFromBasket(int userId, int productId);
    Task<IEnumerable<BasketRepository.BasketProductDto>> SummaryBasket(int userId);
}