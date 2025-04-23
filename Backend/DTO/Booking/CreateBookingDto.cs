namespace Backend.DTO;

public record CreateBookingDto(
    int SailorId,
    int DockId,
    DateOnly StartDate,
    DateOnly EndDate,
    int People,
    string Payment,
    int DockingSpotId
);