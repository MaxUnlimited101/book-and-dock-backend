using System.ComponentModel.DataAnnotations;

namespace Backend.DTO;

public record RoleDTO(
    [Range(0, int.MaxValue)]
    int Id,

    [Required]
    [StringLength(100, MinimumLength = 1)]
    string Name,
    
    DateTime CreatedAt
);