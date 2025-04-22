using System;

namespace Backend.DTO.Review;

public record ReviewDTO(int Id, int UserId, int DockId, double Rating, string Content, DateTime CreatedAt, DateTime? UpdatedAt);