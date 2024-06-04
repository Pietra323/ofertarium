using backend.Data.Models;

namespace backend.Data.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task CreatePaymentCard(PaymentCard paymentCard);
}