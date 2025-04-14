using Backend.Models;

namespace Backend.Interfaces;

public interface IBookingRepository
{
    public int Create(Booking booking);
    public void Delete(int id);
    public bool CheckIfExistsById(int id);
}