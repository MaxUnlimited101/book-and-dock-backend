using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTO;

public record ServiceDto(
    int Id,

    [Required] string Name,

    string? Description,

    [Required] [Range(0, double.MaxValue)] decimal Price,

    [Required] [Range(0, int.MaxValue)] int? PortId,

    [Required] [Range(0, int.MaxValue)] int? DockingSpotId,

    [Required] bool IsAvailable,

    DateTime? CreatedOn
)
{
    public ServiceDto WithId(int id) => this with { Id = id };

    public static ServiceDto FromModel(Service service)
    {
        return new ServiceDto(
            service.Id,
            service.Name,
            service.Description,
            service.Price,
            service.PortId,
            service.DockingSpotId,
            service.IsAvailable,
            service.CreatedOn
        );
    }
}