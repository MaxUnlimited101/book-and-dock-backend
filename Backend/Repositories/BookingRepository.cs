using Backend.Data;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookAndDockContext _context;

    public BookingRepository(BookAndDockContext context)
    {
        _context = context;
    }
    
    public int Create(Booking booking)
    {
        int id = _context.Bookings.Add(booking).Entity.Id;
        _context.SaveChanges();
        return id;
    }

    public void Delete(int id)
    {
        _context.Bookings.Remove(_context.Bookings.Find(id)!);
        _context.SaveChanges();
    }

    public bool CheckIfExistsById(int id)
    {
        return _context.Bookings.Find(id) != null;
    }

    public Booking? GetById(int id)
    {
        return _context.Bookings.Find(id);
    }
    public List<Booking> GetAll()
    {
        return _context.Bookings.ToList();
    }
    public void Update(Booking booking)
    {
        _context.Bookings.Update(booking);
        _context.SaveChanges();
    }
}