using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.DTO;
using Backend.Exceptions;

namespace Backend.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IPortRepository _portRepository;
    private readonly IDockingSpotRepository _dockingSpotRepository;

    public LocationService(ILocationRepository locationRepository, 
                           IPortRepository portRepository,
                           IDockingSpotRepository dockingSpotRepository)
    {
        _portRepository = portRepository;
        _dockingSpotRepository = dockingSpotRepository;
        _locationRepository = locationRepository;
    }

    public async Task CreateLocationAsync(LocationDto location)
    {
        Location l = LocationDto.ToModel(location);
        Port? port = null;
        DockingSpot? dockingSpot = null;
        if (location.PortId != null)
        {
            port = _portRepository.GetById(location.PortId.Value);
            if (port == null)
            {
                throw new ModelInvalidException($"Port with ID {location.PortId} not found.");
            }
        }
        if (location.DockingSpotId != null)
        {
            dockingSpot = _dockingSpotRepository.GetDockingSpotById(location.DockingSpotId.Value);
            if (dockingSpot == null)
            {
                throw new ModelInvalidException($"Docking spot with ID {location.DockingSpotId} not found.");
            }
        }
        if (port != null && dockingSpot != null)
        {
            throw new ModelInvalidException("Location cannot have both PortId and DockingSpotId set.");
        }
        if (port == null && dockingSpot == null)
        {
            throw new ModelInvalidException("Location must have either PortId or DockingSpotId set.");
        }
        if (port != null)
        {
            l.Port = port;
        }
        else
        {
            l.DockingSpot = dockingSpot;
        }
        l.CreatedOn ??= DateTime.UtcNow;
        await _locationRepository.CreateLocationAsync(l);
    }

    public async Task DeleteLocationAsync(int id)
    {
        var location = await _locationRepository.GetLocationByIdAsync(id);
        if (location == null)
        {
            throw new ModelInvalidException($"Location with ID {id} not found.");
        }
        await _locationRepository.DeleteLocationAsync(id);
    }

    public async Task<IEnumerable<LocationDto>> GetAllLocationsAsync()
    {
        var locations = await _locationRepository.GetAllLocationsAsync();
        return locations.Select(l => LocationDto.FromModel(l));
    }

    public async Task<LocationDto?> GetLocationByIdAsync(int id)
    {
        var location = await _locationRepository.GetLocationByIdAsync(id);
        if (location == null)
        {
            throw new ModelInvalidException($"Location with ID {id} not found.");
        }
        return LocationDto.FromModel(location);
    }

    public async Task UpdateLocationAsync(LocationDto location)
    {
        Location l = LocationDto.ToModel(location);
        Port? port = null;
        DockingSpot? dockingSpot = null;
        if (location.PortId != null)
        {
            port = _portRepository.GetById(location.PortId.Value);
            if (port == null)
            {
                throw new ModelInvalidException($"Port with ID {location.PortId} not found.");
            }
        }
        if (location.DockingSpotId != null)
        {
            dockingSpot = _dockingSpotRepository.GetDockingSpotById(location.DockingSpotId.Value);
            if (dockingSpot == null)
            {
                throw new ModelInvalidException($"Docking spot with ID {location.DockingSpotId} not found.");
            }
        }
        if (port != null && dockingSpot != null)
        {
            throw new ModelInvalidException("Location cannot have both PortId and DockingSpotId set.");
        }
        if (port == null && dockingSpot == null)
        {
            throw new ModelInvalidException("Location must have either PortId or DockingSpotId set.");
        }
        if (port != null)
        {
            l.Port = port;
        }
        else
        {
            l.DockingSpot = dockingSpot;
        }
        l.CreatedOn ??= DateTime.UtcNow;
        await _locationRepository.UpdateLocationAsync(l);
    }
}