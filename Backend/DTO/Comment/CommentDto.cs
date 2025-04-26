using System.ComponentModel.DataAnnotations;

namespace Backend.DTO;

public record CommentDto(
    int Id,
    [Required] int CreatedBy,
    [Required] int GuideId,
    [Required] string Content,
    DateTime? CreatedOn
);