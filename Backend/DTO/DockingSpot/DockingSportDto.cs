using System.ComponentModel.DataAnnotations;

namespace Backend.DTO;

public record DockingSpotDto(
    int Id,
    string? Name,
    string? Description,
    [Required] int OwnerId,
    [Required] int PortId,
    double? PricePerNight,
    double? PricePerPerson,
    [Required] bool IsAvailable,
    DateTime? CreatedOn
);