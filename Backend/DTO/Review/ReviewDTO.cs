using System;

namespace Backend.DTO;

public record ReviewDTO(int Id, int UserId, int DockId, double Rating, string Content, DateTime CreatedAt, DateTime? UpdatedAt);