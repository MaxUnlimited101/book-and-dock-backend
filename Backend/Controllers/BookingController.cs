using Backend.DTO;
using Backend.DTO.Booking;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
            int id = await _bookingService.CreateAsync(createBookingDto);
            return await Task.FromResult(
                Ok(new StatusReturnDto("Booking created successfully, id given", id)));
        }
        catch (ModelAlreadyExistsException e)
        {
            return await Task.FromResult(StatusCode(400, new StatusReturnDto(e.Message, null)));
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

    [HttpGet]
    public async Task<ActionResult<List<Booking>>> GetAll()
    {
        var bookings = _bookingService.GetAll();
        return await Task.FromResult(Ok(bookings));
    }
    
    [HttpGet("my")]
    public async Task<ActionResult<List<Booking>>> GetMyBookings()
    {
        if (!int.TryParse(User.Identity?.Name, out int userId))
        {
            return await Task.FromResult(Unauthorized("Invalid or missing user identity"));
        }

        var bookings = _bookingService.GetBookingsByUserId(userId);
        return await Task.FromResult(Ok(bookings));
    }

    [HttpPut("{id}")]
    public ActionResult<StatusReturnDto> Update(int id, [FromBody] UpdateBookingDto dto)
    {
        try
        {
            _bookingService.Update(id, dto);
            return Ok(new StatusReturnDto("Booking updated", id));
        }
        catch (ModelNotFoundException e)
        {
            return NotFound(new StatusReturnDto(e.Message, null));
        }
        catch (ModelInvalidException e)
        {
            return BadRequest(new StatusReturnDto(e.Message, null));
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetById(int id)
    {
        try
        {
            var booking = _bookingService.GetBookingById(id);
            return await Task.FromResult(Ok(booking));
        }
        catch (ModelNotFoundException e)
        {
            return await Task.FromResult(NotFound(new StatusReturnDto(e.Message, null)));
        }
    }

}