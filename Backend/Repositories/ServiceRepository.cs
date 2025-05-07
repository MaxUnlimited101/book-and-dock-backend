using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly BookAndDockContext _context;

    public ServiceRepository(BookAndDockContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Service>> GetAllServicesAsync()
    {
        return await _context.Services.ToListAsync();
    }

    public async Task<Service?> GetServiceByIdAsync(int id)
    {
        return await _context.Services.FindAsync(id);
    }

    public async Task<int> CreateServiceAsync(Service service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return service.Id;
    }

    public async Task UpdateServiceAsync(Service service)
    {
        _context.Services.Update(service);
        await _context.SaveChangesAsync();
    }

    public Task DeleteServiceAsync(int id)
    {
        var service = _context.Services.Find(id);
        if (service != null)
        {
            _context.Services.Remove(service);
            return _context.SaveChangesAsync();
        }
        return Task.CompletedTask;
    }
}