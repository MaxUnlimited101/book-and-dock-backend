using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.Review;

public record CreateReviewDTO(
    [Required] int UserId, 
    [Required] int DockId, 
    [Required] int Rating, 
    [Required] string Content
);