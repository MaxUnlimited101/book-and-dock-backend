using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserService _userRepository;
    private readonly IDockingSpotService _dockService;

    public BookingService(IBookingRepository bookingRepository, IUserService userService, IDockingSpotService dockService)
    {
        _bookingRepository = bookingRepository;
        _userRepository = userService;
        _dockService = dockService;
    }
    
    public int Create(CreateBookingDto booking) 
    {
        Booking b = new();

        if (!_userRepository.CheckIfUserExistsByIdAndRole(booking.SailorId, new Role()))
        {
            throw new ModelNotFoundException("Invalid user");
        }

        if (!_dockService.CheckIfDockExistsById(booking.DockingSpotId))
        {
            throw new ModelNotFoundException("Invalid dock");
        }

        if (booking.EndDate <= booking.StartDate)
        {
            throw new InvalidDataException("End date cannot be earlier than start date");
        }
        
        return _bookingRepository.Create(b);
    }

    public void Delete(int id)
    {
        if (_bookingRepository.CheckIfExistsById(id))
        {
            _bookingRepository.Delete(id);
        }
        else
        {
            throw new ModelNotFoundException("Invalid booking id");
        }
    }
}