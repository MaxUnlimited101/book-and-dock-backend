using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class PaymentMethodService : IPaymentMethodService
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    
    public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
    {
        _paymentMethodRepository = paymentMethodRepository;
    }
    
    public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync()
    {
        return await _paymentMethodRepository.GetAllPaymentMethodsAsync();
    }

    public async Task<PaymentMethod?> GetPaymentMethodByIdAsync(int id)
    {
        return await _paymentMethodRepository.GetPaymentMethodByIdAsync(id);
    }

    public async Task<int> CreatePaymentMethodAsync(PaymentMethodDTO paymentMethod)
    {
        var newPaymentMethod = new PaymentMethod
        {
            Name = paymentMethod.Name,
            CreatedOn = paymentMethod.CreatedAt
        };

        int newId = await _paymentMethodRepository.CreatePaymentMethodAsync(newPaymentMethod);
        return newId;
    }

    public async Task UpdatePaymentMethodAsync(int id, PaymentMethodDTO updatedPaymentMethod)
    {
        var existingPaymentMethod = await _paymentMethodRepository.GetPaymentMethodByIdAsync(id);
        if (existingPaymentMethod == null)
            throw new ModelInvalidException("Payment method not found");

        existingPaymentMethod.Name = updatedPaymentMethod.Name;
        existingPaymentMethod.CreatedOn = updatedPaymentMethod.CreatedAt;

        await _paymentMethodRepository.UpdatePaymentMethodAsync(existingPaymentMethod);
    }

    public async Task DeletePaymentMethodAsync(int id)
    {
        var existingPaymentMethod = await _paymentMethodRepository.GetPaymentMethodByIdAsync(id);
        if (existingPaymentMethod == null)
            throw new ModelInvalidException("Payment method not found");

        await _paymentMethodRepository.DeletePaymentMethodAsync(existingPaymentMethod.Id);
    }
}