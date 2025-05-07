using Backend.Models;

namespace Backend.Interfaces;

public interface IPaymentMethodRepository
{
    Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync();
    Task<PaymentMethod?> GetPaymentMethodByIdAsync(int id);
    Task<int> CreatePaymentMethodAsync(PaymentMethod paymentMethod);
    Task UpdatePaymentMethodAsync(PaymentMethod updatedPaymentMethod);
    Task DeletePaymentMethodAsync(int id);
}