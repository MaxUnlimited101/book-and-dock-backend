using Backend.Models;

namespace Backend.DTO;

public record DockingSpotReturnDto(
    int Id,
    string? Name,
    string? Description,
    int OwnerId,
    int PortId,
    double? PricePerNight,
    double? PricePerPerson,
    bool IsAvailable,
    DateTime? CreatedOn,
    ICollection<Location> Locations,
    User Owner,
    Port Port,
    ICollection<Service> Services
)
{
    public static DockingSpotReturnDto FromModel(DockingSpot spot)
    {
        return new DockingSpotReturnDto(
            Id: spot.Id,
            Name: spot.Name,
            Description: spot.Description,
            OwnerId: spot.OwnerId,
            PortId: spot.PortId,
            PricePerNight: spot.PricePerNight,
            PricePerPerson: spot.PricePerPerson,
            IsAvailable: spot.IsAvailable,
            CreatedOn: spot.CreatedOn,
            Locations: spot.Locations?.ToList() ?? new List<Location>(), 
            Owner: spot.Owner,
            Port: spot.Port,
            Services: spot.Services?.ToList() ?? new List<Service>() 
        );
    }
}