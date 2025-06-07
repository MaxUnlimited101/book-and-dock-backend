namespace Backend.DTO.Booking
{
    public record UpdateBookingDto(
    DateOnly StartDate,
    DateOnly EndDate,
    int People,
    string PaymentMethod
);

}
