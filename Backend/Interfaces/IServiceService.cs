using Backend.DTO;
using Backend.Models;

namespace Backend.Interfaces;

public interface IServiceService
{
    Task<IEnumerable<Service>> GetAllServicesAsync();
    Task<Service?> GetServiceByIdAsync(int id);
    Task<int> CreateServiceAsync(ServiceDto dto);
    Task UpdateServiceAsync(ServiceDto dto);
    Task DeleteServiceAsync(int id);
}