using Backend.Data;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class DockingSpotService : IDockingSpotService
{
    private readonly IDockingSpotRepository _dockingSpotRepository;
    private readonly IPortRepository _portRepository;
    private readonly IUserRepository _userRepository;
    
    public DockingSpotService(IDockingSpotRepository dockRepository, IPortRepository portRepository, IUserRepository userRepository)
    {
        _portRepository = portRepository;
        _userRepository = userRepository;
        _dockingSpotRepository = dockRepository;
    }

    public Task<List<DockingSpot>> GetAvailableDockingSpotsAsync(string? location, DateTime? date, decimal? price,
        List<string>? services, string? availability)
    {
        return _dockingSpotRepository.GetAvailableDockingSpotsAsync(location, date, price, services, availability);
    }

    public bool CheckIfDockingSpotExistsById(int id)
    {
        return _dockingSpotRepository.CheckIfDockingSpotExistsById(id);
    }

    public DockingSpot? GetDockingSpotById(int id)
    {
        var dock = _dockingSpotRepository.GetDockingSpotById(id);
        if (dock == null)
        {
            throw new ModelInvalidException("DockingSpot not found");
        }
        return dock;
    }

    public void CreateDockingSpot(DockingSpotDto ds)
    {
        DockingSpot dockingSpot = new();

        var owner = _userRepository.GetUserById(ds.OwnerId);
        if (owner == null)
        {
            throw new ModelInvalidException("Owner not found");
        }
        dockingSpot.OwnerId = ds.OwnerId;
        dockingSpot.Owner = owner;

        var port = _portRepository.GetById(ds.PortId);
        if (port == null)
        {
            throw new ModelInvalidException("Port not found");
        }
        dockingSpot.Port = port;
        dockingSpot.PortId = ds.PortId;

        dockingSpot.CreatedOn = ds.CreatedOn;
        dockingSpot.Description = ds.Description;
        dockingSpot.IsAvailable = ds.IsAvailable;
        dockingSpot.Name = ds.Name;
        dockingSpot.PricePerNight = ds.PricePerNight;
        dockingSpot.PricePerPerson = ds.PricePerPerson;

        _dockingSpotRepository.CreateDockingSpot(dockingSpot);
    }

    public void UpdateDockingSpot(int id, DockingSpotDto ds)
    {
        var dockingSpot = _dockingSpotRepository.GetDockingSpotById(id);
        if (dockingSpot == null)
        {
            throw new ModelInvalidException("DockingSpot not found");
        }
        var owner = _userRepository.GetUserById(ds.OwnerId);
        if (owner == null)
        {
            throw new ModelInvalidException("Owner not found");
        }
        dockingSpot.OwnerId = ds.OwnerId;
        dockingSpot.Owner = owner;

        var port = _portRepository.GetById(ds.PortId);
        if (port == null)
        {
            throw new ModelInvalidException("Port not found");
        }
        dockingSpot.Port = port;
        dockingSpot.PortId = ds.PortId;

        dockingSpot.CreatedOn = ds.CreatedOn;
        dockingSpot.Description = ds.Description;
        dockingSpot.IsAvailable = ds.IsAvailable;
        dockingSpot.Name = ds.Name;
        dockingSpot.PricePerNight = ds.PricePerNight;
        dockingSpot.PricePerPerson = ds.PricePerPerson;

        _dockingSpotRepository.UpdateDockingSpot(dockingSpot);
    }

    public void DeleteDockingSpot(int id)
    {
        var dock = _dockingSpotRepository.GetDockingSpotById(id);
        if (dock == null)
        {
            throw new ModelInvalidException("Dock not found");
        }
        _dockingSpotRepository.DeleteDockingSpot(id);
    }
}