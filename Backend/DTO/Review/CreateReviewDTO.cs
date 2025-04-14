namespace Backend.DTO.Review;

public record CreateReviewDTO(int UserId, int DockId, int Rating, string Content);