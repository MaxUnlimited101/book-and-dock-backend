using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly BookAndDockContext _context;

    public PaymentMethodRepository(BookAndDockContext context)
    {
        _context = context;
    }

    public PaymentMethod? GetPaymentMethodById(int id)
    {
        return _context.PaymentMethods.FirstOrDefault(x => x.Id == id);
    }

    public async Task<int> CreatePaymentMethodAsync(PaymentMethod paymentMethod)
    {
        _context.PaymentMethods.Add(paymentMethod);
        await _context.SaveChangesAsync();
        return paymentMethod.Id;
    }

    public async Task DeletePaymentMethodAsync(int id)
    {
        var paymentMethod = await _context.PaymentMethods.FindAsync(id);
        if (paymentMethod != null)
        {
            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();
        }
    }

    public PaymentMethod? GetPaymentMethodByName(string name)
    {
        IEnumerable<PaymentMethod> paymentMethods = _context.PaymentMethods.AsEnumerable();
        return paymentMethods.First(pm => String.Compare(pm.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync()
    {
        return await _context.PaymentMethods.ToListAsync();
    }

    public async Task<PaymentMethod?> GetPaymentMethodByIdAsync(int id)
    {
        return await _context.PaymentMethods.FindAsync(id);
    }

    public async Task UpdatePaymentMethodAsync(PaymentMethod paymentMethod)
    {
        _context.PaymentMethods.Update(paymentMethod);
        await _context.SaveChangesAsync();
    }
}