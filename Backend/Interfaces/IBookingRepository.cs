using Backend.Models;

namespace Backend.Interfaces;

public interface IBookingRepository
{
    public int Create(Booking booking);
    public void Delete(int id);
    public Booking? GetById(int id);
    public List<Booking> GetAll();
    public void Update(Booking booking);
    public bool CheckIfExistsById(int id);
}