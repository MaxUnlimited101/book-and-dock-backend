using Backend.Data;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class DockingSpotService : IDockingSpotService
{
    private readonly IDockingSpotRepository _dockRepository;
    
    public DockingSpotService(IDockingSpotRepository dockRepository)
    {
        _dockRepository = dockRepository;
    }

    public Task<List<DockingSpot>> GetAvailableDocksAsync(string? location, DateTime? date, decimal? price,
        List<string>? services, string? availability)
    {
        return _dockRepository.GetAvailableDocksAsync(location, date, price, services, availability);
    }

    public bool CheckIfDockExistsById(int id)
    {
        return _dockRepository.CheckIfDockExistsById(id);
    }
}