using Backend.DTO;
using Backend.DTO.Review;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("docks")]
public class DockController : ControllerBase
{
    private readonly IDockService _dockService;

    public DockController(IDockService dockService)
    {
        _dockService = dockService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DockingSpotReturnDto>>> GetAvailableDocks(
        [FromQuery] string? location,
        [FromQuery] DateTime? date,
        [FromQuery] decimal? price,
        [FromQuery] string? services,
        [FromQuery] string? availability)
    {
        List<string>? servicesList = null;
        if (!string.IsNullOrEmpty(services))
            servicesList = services.Split(",").ToList();
        var docks = await _dockService.GetAvailableDocksAsync(location, date, price, servicesList, availability);
        return Ok(docks.Select(ds => DockingSpotReturnDto.FromModel(ds)).ToList());
    }

    [HttpPost("{id}/reviews")]
    public async Task<ActionResult<StatusReturnDto>> CreateReview([FromRoute] int id, [FromBody] CreateReviewDTO reviewDto)
    {
        throw new NotImplementedException();
    }
}