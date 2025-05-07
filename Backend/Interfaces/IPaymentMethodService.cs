using Backend.DTO;
using Backend.Models;

namespace Backend.Interfaces;

public interface IPaymentMethodService
{
    Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync();
    Task<PaymentMethod?> GetPaymentMethodByIdAsync(int id);
    Task<int> CreatePaymentMethodAsync(PaymentMethodDTO paymentMethod);
    Task UpdatePaymentMethodAsync(int id, PaymentMethodDTO updatedPaymentMethod);
    Task DeletePaymentMethodAsync(int id);
}