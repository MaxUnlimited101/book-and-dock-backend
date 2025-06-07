using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class DockingSpotRepository : IDockingSpotRepository
{
    private readonly BookAndDockContext _context;

    public DockingSpotRepository(BookAndDockContext context)
    {
        _context = context;
    }

    public Task<List<DockingSpot>> GetAvailableDockingSpotsAsync(string? location = null!,
        DateTime? date = null, decimal? price = null, List<string>? services = null!,
        string? availability = null!)
    {
        var query = _context.DockingSpots.AsQueryable();

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(d =>
                d.Locations.First().Town.Contains(location, StringComparison.InvariantCultureIgnoreCase));
        }

        if (date.HasValue)
        {
            query = query.Where(d => !d.Bookings.Any(b =>
                b.StartDate.ToDateTime(TimeOnly.MinValue) <= date.Value.Date &&
                b.EndDate.ToDateTime(TimeOnly.MinValue) >= date.Value.Date));
        }

        if (price.HasValue)
        {
            query = query.Where(d => _context.DockingSpots
                .Any(ds => ds.PricePerNight != null && (decimal)ds.PricePerNight.Value <= price.Value));
        }

        if (services != null && services.Any())
        {
            var _services = _context.Services.AsQueryable();
            query = query.Join(_services, d => d.Id, ds => ds.DockingSpotId, (s, d) => s);
            foreach (var service in services)
            {
                query = query.Where(d => d.Services.Any((s) => s.Name == service && s.DockingSpotId == s.Id));
            }
        }

        if (!string.IsNullOrEmpty(availability))
        {
            query = query.Where(d => d.IsAvailable);
        }

        return query.ToListAsync();
    }

    public bool CheckIfDockingSpotExistsById(int id)
    {
        if (_context.DockingSpots.Any(d => d.Id == id))
        {
            return true;
        }

        return false;
    }

    public DockingSpot? GetDockingSpotById(int id)
    {
        return _context.DockingSpots.Any(d => d.Id == id)
            ? _context.DockingSpots.FirstOrDefault(d => d.Id == id)
            : null!;
    }

    public List<DockingSpot> GetAllDockingSpots()
    {
        return _context.DockingSpots.ToList();
    }

    public void UpdateDockingSpot(DockingSpot dock)
    {
        _context.DockingSpots.Update(dock);
        _context.SaveChanges();
    }

    public int CreateDockingSpot(DockingSpot dock)
    {
        int id = _context.DockingSpots.Add(dock).Entity.Id;
        _context.SaveChanges();
        return id;
    }

    public void DeleteDockingSpot(int id)
    {
        _context.DockingSpots.Remove(_context.DockingSpots.Find(id)!);
        _context.SaveChanges();
    }
}