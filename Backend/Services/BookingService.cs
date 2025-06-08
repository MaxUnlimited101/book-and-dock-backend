using Backend.DTO;
using Backend.DTO.Booking;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserService _userRepository;
    private readonly IDockingSpotService _dockService;
    private readonly IPaymentMethodService _paymentMethodService;

    public BookingService(IBookingRepository bookingRepository, IUserService userService,
        IDockingSpotService dockService, IPaymentMethodService paymentMethodService)
    {
        _bookingRepository = bookingRepository;
        _userRepository = userService;
        _dockService = dockService;
        _paymentMethodService = paymentMethodService;
    }

    public async Task<int> CreateAsync(CreateBookingDto booking)
    {
        Booking b = new();

        if (!_userRepository.CheckIfUserExistsById(booking.SailorId))
        {
            throw new ModelNotFoundException("Invalid user");
        }

        if (!_dockService.CheckIfDockingSpotExistsById(booking.DockingSpotId))
        {
            throw new ModelNotFoundException("Invalid dock");
        }

        if (booking.EndDate <= booking.StartDate)
        {
            throw new InvalidDataException("End date cannot be earlier than start date");
        }

        b.SailorId = booking.SailorId;
        b.DockingSpotId = booking.DockingSpotId;
        b.StartDate = booking.StartDate;
        b.EndDate = booking.EndDate;
        b.People = booking.People;
        
        // this might throw
        b.PaymentMethod = await _paymentMethodService.GetPaymentMethodByNameAsync(booking.Payment);
        
        b.PaymentMethodId = b.PaymentMethod.Id;
        b.CreatedOn = DateTime.UtcNow;
        b.IsPaid = false;

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

    public List<Booking> GetAll() => _bookingRepository.GetAll();

    public List<Booking> GetBookingsByUserId(int userId) =>
        _bookingRepository.GetAll().Where(b => b.SailorId == userId).ToList();

    public void Update(int id, UpdateBookingDto dto)
    {
        var booking = _bookingRepository.GetById(id)
                      ?? throw new ModelNotFoundException("Booking not found");

        if (dto.EndDate <= dto.StartDate)
            throw new InvalidDataException("End date cannot be before start date");

        booking.StartDate = dto.StartDate;
        booking.EndDate = dto.EndDate;
        booking.People = dto.People;
        booking.PaymentMethod = new PaymentMethod { Name = dto.PaymentMethod };

        _bookingRepository.Update(booking);
    }

    public Booking? GetBookingById(int id)
    {
        return _bookingRepository.GetById(id);
    }
}