using Backend.Models;
using Backend.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<LocationDto>> GetAllLocationsAsync();
    Task<LocationDto?> GetLocationByIdAsync(int id);
    Task CreateLocationAsync(LocationDto location);
    Task UpdateLocationAsync(int id, LocationDto location);
    Task DeleteLocationAsync(int id);
}