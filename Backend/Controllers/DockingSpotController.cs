using Backend.DTO;
using Backend.DTO.Review;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DockingSpotController : ControllerBase
{
    private readonly IDockingSpotService _dockService;

    public DockingSpotController(IDockingSpotService dockService)
    {
        _dockService = dockService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DockingSpotReturnDto>>> GetAvailableDockingSpots(
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

    // [HttpPost("{id}/reviews")]
    // public ActionResult<StatusReturnDto> CreateReview([FromRoute] int id, [FromBody] CreateReviewDTO reviewDto)
    // {
        
    // }

    // [HttpPost]
    // public ActionResult<StatusReturnDto> CreateDockingSpot([FromBody] DockingSpotCreateDto dockingSpotDto)
    // {
        
    // }

    // [HttpPut("{id}")]
    // public ActionResult<StatusReturnDto> UpdateDockingSpot([FromRoute] int id, [FromBody] DockingSpotUpdateDto dockingSpotDto)
    // {
        
    // }

    // [HttpDelete("{id}")]
    // public ActionResult<StatusReturnDto> DeleteDockingSpot([FromRoute] int id)
    // {
        
    // }

    // [HttpGet("{id}")]
    // public ActionResult<DockingSpotReturnDto> GetDockingSpot([FromRoute] int id)
    // {
    //     var dockingSpot = _dockService.GetDockingSpotById(id);
    //     if (dockingSpot == null)
    //         return NotFound();
    //     return Ok(DockingSpotReturnDto.FromModel(dockingSpot));
    // }

    // [HttpGet("{id}/reviews")]
    // public ActionResult<IEnumerable<ReviewReturnDto>> GetDockingSpotReviews([FromRoute] int id)
    // {
    //     var reviews = _dockService.GetDockingSpotReviews(id);
    //     if (reviews == null)
    //         return NotFound();
    //     return Ok(reviews.Select(r => ReviewReturnDto.FromModel(r)).ToList());
    // }
}