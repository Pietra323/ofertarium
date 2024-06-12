using backend.Data.Models;

namespace backend.Data.Repositories.Interfaces;

public interface IOrderRepository
{
    Task TransferBasketToHistory(int userId, int paymentCardId);
    Task<IEnumerable<History>> GetUserOrderHistory(int userId);

    Task<byte[]> GenerateOrderPdfAsync(int id);
}