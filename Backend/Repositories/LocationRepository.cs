using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly BookAndDockContext _context;

    public LocationRepository(BookAndDockContext context)
    {
        _context = context;
    }

    public async Task CreateLocationAsync(Location location)
    {
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
    }

    public Task DeleteLocationAsync(int id)
    {
        var location = _context.Locations.Find(id);
        if (location != null)
        {
            _context.Locations.Remove(location);
            return _context.SaveChangesAsync();
        }
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Location>> GetAllLocationsAsync()
    {
        return await _context.Locations.ToListAsync();
    }

    public async Task<Location?> GetLocationByIdAsync(int id)
    {
        return await _context.Locations.FindAsync(id);
    }

    public async Task UpdateLocationAsync(Location location)
    {
        _context.Locations.Update(location);
        await _context.SaveChangesAsync();
    }
}