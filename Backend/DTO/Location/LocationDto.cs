using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTO;

public record LocationDto(
    int Id,
    DateTime? CreatedOn,
    [Required] double Latitude,
    [Required] double Longitude,
    [Required] string Town,
    int? PortId,
    int? DockingSpotId
)
{
    static public LocationDto FromModel(Location location)
    {
        return new LocationDto(
            location.Id,
            location.CreatedOn,
            location.Latitude,
            location.Longitude,
            location.Town,
            location.PortId,
            location.DockingSpotId
        );
    }

    /// <summary>
    /// Does not validate the model
    /// </summary>
    static public Location ToModel(LocationDto location)
    {
        return new Location{
            Id = location.Id,
            CreatedOn = location.CreatedOn,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Town = location.Town,
            PortId = location.PortId,
            DockingSpotId = location.DockingSpotId
        };
    }
}