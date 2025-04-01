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
}