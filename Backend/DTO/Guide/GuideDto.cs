using System.ComponentModel.DataAnnotations;

namespace Backend.DTO;

public record GuideDto(
    int Id,
    [Required] string Title,
    [Required] string Content,
    [Required] int CreatedBy,
    DateTime? CreatedOn,
    [Required] bool IsApproved
);