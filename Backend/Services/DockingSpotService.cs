using Backend.Data;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class DockingSpotService : IDockingSpotService
{
    private readonly IDockingSpotRepository _dockingSpotRepository;
    
    public DockingSpotService(IDockingSpotRepository dockRepository)
    {
        _dockingSpotRepository = dockRepository;
    }

    public Task<List<DockingSpot>> GetAvailableDocksAsync(string? location, DateTime? date, decimal? price,
        List<string>? services, string? availability)
    {
        return _dockingSpotRepository.GetAvailableDocksAsync(location, date, price, services, availability);
    }

    public bool CheckIfDockExistsById(int id)
    {
        return _dockingSpotRepository.CheckIfDockExistsById(id);
    }

    public DockingSpot? GetDockById(int id)
    {
        var dock = _dockingSpotRepository.GetDockById(id);
        if (dock == null)
        {
            throw new ModelInvalidException("Dock not found");
        }
        return dock;
    }

    public void CreateDock(DockingSpotDto ds)
    {
        throw new NotImplementedException();
    }

    public void UpdateDock(DockingSpotDto ds)
    {
        throw new NotImplementedException();
    }

    public void DeleteDock(int id)
    {
        var dock = _dockingSpotRepository.GetDockById(id);
        if (dock == null)
        {
            throw new ModelInvalidException("Dock not found");
        }
        _dockingSpotRepository.DeleteDock(id);
    }
}