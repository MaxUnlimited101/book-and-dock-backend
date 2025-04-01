using Backend.DTO;
using Backend.Exceptions;

namespace Backend.Interfaces;

public interface IBookingService
{
    /// <summary>
    /// Tries to create the booking from the related DTO object
    /// </summary>
    /// <returns>The id of the created booking</returns>
    /// <exception cref="ModelNotFoundException">One of the required models for
    /// creating the booking is not present in the DBContext</exception>
    int Create(CreateBookingDto booking);
}