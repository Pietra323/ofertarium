using backend.Data.Models;
using backend.Data.Models.DataBase;
using backend.Data.Models.ManyToManyConnections;
using backend.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly DataBase _ctx;

    public OrderRepository(DataBase ctx)
    {
        _ctx = ctx;
    }
    
    public async Task TransferBasketToHistory(int userId, int paymentCardId)
    {
        var basketProducts = await _ctx.BasketProducts
            .Where(bp => bp.BasketId == userId)
            .Include(bp => bp.product)
            .ToListAsync();

        if (!basketProducts.Any())
        {
            throw new ArgumentException("Basket is empty.");
        }

        var paymentCard = await _ctx.Payments.FirstOrDefaultAsync(pc => pc.UserId == userId);

        if (paymentCard == null)
        {
            throw new ArgumentException("Payment card not found.");
        }

        decimal totalAmount = basketProducts.Sum(bp => bp.product.Price * bp.quantity);

        if (paymentCard.Balance < totalAmount)
        {
            throw new InvalidOperationException("Insufficient funds on the payment card.");
        }

        paymentCard.Balance -= totalAmount;

        int orderId = await _ctx.Histories.MaxAsync(h => (int?)h.OrderId) ?? 0;
        orderId++;

        var histories = basketProducts.Select(bp => new History
        {
            OrderId = orderId,
            UserId = bp.BasketId,
            ProductId = bp.ProductId,
            OrderDate = DateTime.UtcNow,
            Quantity = bp.quantity,
            ProductName = bp.product.ProductName
        }).ToList();

        _ctx.Histories.AddRange(histories);
        
        var order = new Order
        {
            Name = $"Zamówienie{orderId}, użytkownia {paymentCard.User.Name}",
            UserId = userId,
            OrderProducts = basketProducts.Select(bp => new OrderProduct
            {
                ProductId = bp.ProductId,
                Quantity = bp.quantity
            }).ToList()
        };
        
        _ctx.Orders.Add(order);
        _ctx.BasketProducts.RemoveRange(basketProducts);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<History>> GetUserOrderHistory(int userId)
    {
        var histories = await _ctx.Histories
            .Where(h => h.UserId == userId)
            .Include(h => h.Product)
            .ToListAsync();

        var historyDTOs = histories.Select(h => new History()
        {
            OrderId = h.OrderId,
            UserId = h.UserId,
            ProductId = h.ProductId,
            OrderDate = h.OrderDate,
            Quantity = h.Quantity,
            ProductName = h.Product.ProductName
        });

        return historyDTOs;
    }
}