using Backend.Models;

namespace Backend.Interfaces;

public interface IBookingRepository
{
    public int Create(Booking booking);
}