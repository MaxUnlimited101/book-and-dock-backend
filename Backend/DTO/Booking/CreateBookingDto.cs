namespace Backend.DTO;

public record CreateBookingDto(
    int SailorId,
    DateTime StartDate,
    DateTime EndDate,
    int People,
    string Payment,
    int DockingSpotId
);