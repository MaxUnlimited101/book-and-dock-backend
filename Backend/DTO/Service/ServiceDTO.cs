using System.ComponentModel.DataAnnotations;

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
}