using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    
    [HttpPost]
    public ActionResult<StatusReturnDto> CreateBooking([FromBody]CreateBookingDto createBookingDto)
    {
        try
        {
            int id = _bookingService.Create(createBookingDto);
            return Ok(new StatusReturnDto("Booking created successfully, id given", id));
        }
        catch (ModelAlreadyExistsException e)
        {
            return StatusCode(500, new StatusReturnDto(e.Message, null));
        }
    }
}