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
    public async Task<ActionResult<StatusReturnDto>> Create([FromBody] CreateBookingDto createBookingDto)
    {
        try
        {
            int id = _bookingService.Create(createBookingDto);
            return await Task.FromResult(
                Ok(new StatusReturnDto("Booking created successfully, id given", id)));
        }
        catch (ModelAlreadyExistsException e)
        {
            return await Task.FromResult(StatusCode(401, new StatusReturnDto(e.Message, null)));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<StatusReturnDto>> Delete(int id)
    {
        try
        {
            _bookingService.Delete(id);
            return await Task.FromResult(Ok(new StatusReturnDto("Booking deleted successfully, id given", id)));
        }
        catch (ModelNotFoundException e)
        {
            return await Task.FromResult(StatusCode(401, new StatusReturnDto(e.Message, null)));
        }
    }
}