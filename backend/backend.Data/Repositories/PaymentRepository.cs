using backend.Data.Models;
using backend.Data.Models.DataBase;
using backend.Data.Repositories.Interfaces;

namespace backend.Data.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly DataBase _ctx;
    
    public PaymentRepository(DataBase ctx)
    {
        _ctx = ctx;
    }
    public async Task CreatePaymentCard(PaymentCard paymentCard)
    {
        _ctx.Payments.Add(paymentCard);
        await _ctx.SaveChangesAsync();
    }
}