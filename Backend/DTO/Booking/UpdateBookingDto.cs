namespace Backend.DTO.Booking
{
    public record UpdateBookingDto(
        DateTime StartDate,
        DateTime EndDate,
        int People,
        string PaymentMethod
    );
}