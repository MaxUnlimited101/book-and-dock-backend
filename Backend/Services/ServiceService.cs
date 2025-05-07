

using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IPortRepository _portRepository;
    private readonly IDockingSpotRepository _dockingSpotRepository;

    public ServiceService(IServiceRepository serviceRepository, IPortRepository portRepository, IDockingSpotRepository dockingSpotRepository)
    {
        _portRepository = portRepository;
        _dockingSpotRepository = dockingSpotRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<int> CreateServiceAsync(ServiceDto dto)
    {
        var service = new Service
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CreatedOn = dto.CreatedOn,
            DockingSpotId = dto.DockingSpotId,
            IsAvailable = dto.IsAvailable,
            PortId = dto.PortId,
        };
        if (dto.DockingSpotId != null)
        {
            var dockingSpot = _dockingSpotRepository.GetDockingSpotById(dto.DockingSpotId.Value);
            if (dockingSpot == null)
            {
                throw new ModelInvalidException("Docking spot not found");
            }
            service.DockingSpot = dockingSpot;
        }
        if (dto.PortId != null)
        {
            var port = _portRepository.GetById(dto.PortId.Value);
            if (port == null)
            {
                throw new ModelInvalidException("Port not found");
            }
            service.Port = port;
        }

        return await _serviceRepository.CreateServiceAsync(service);
    }

    public async Task DeleteServiceAsync(int id)
    {
        var service = await _serviceRepository.GetServiceByIdAsync(id);
        if (service == null)
        {
            throw new ModelInvalidException("Service not found");
        }

        await _serviceRepository.DeleteServiceAsync(id);
    }

    public async Task<IEnumerable<Service?>> GetAllServicesAsync()
    {
        return await _serviceRepository.GetAllServicesAsync();
    }

    public async Task<Service?> GetServiceByIdAsync(int id)
    {
        var service = await _serviceRepository.GetServiceByIdAsync(id);
        if (service == null)
        {
            throw new ModelInvalidException("Service not found");
        }
        return service;
    }

    public async Task UpdateServiceAsync(ServiceDto dto)
    {
        var service = await _serviceRepository.GetServiceByIdAsync(dto.Id);
        if (service == null)
        {
            throw new ModelInvalidException("Service not found");
        }

        service.Name = dto.Name;
        service.Description = dto.Description;
        service.Price = dto.Price;
        service.CreatedOn = dto.CreatedOn;
        service.DockingSpotId = dto.DockingSpotId;
        service.IsAvailable = dto.IsAvailable;
        service.PortId = dto.PortId;

        if (dto.DockingSpotId != null)
        {
            var dockingSpot = _dockingSpotRepository.GetDockingSpotById(dto.DockingSpotId.Value);
            if (dockingSpot == null)
            {
                throw new ModelInvalidException("Docking spot not found");
            }
            service.DockingSpot = dockingSpot;
        }
        if (dto.PortId != null)
        {
            var port = _portRepository.GetById(dto.PortId.Value);
            if (port == null)
            {
                throw new ModelInvalidException("Port not found");
            }
            service.Port = port;
        }

        await _serviceRepository.UpdateServiceAsync(service);
    }
}