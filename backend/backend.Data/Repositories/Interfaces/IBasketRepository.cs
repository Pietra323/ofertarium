using backend.Data.Models;

namespace backend.Data.Repositories.Interfaces;

public interface IBasketRepository
{
    Task AddToBasket(int userId, int productId);
}