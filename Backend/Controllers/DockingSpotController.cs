using Backend.DTO;
using Backend.DTO.Review;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;

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

    [HttpPost]
    public IActionResult CreateDockingSpot([FromBody] DockingSpotDto ds)
    {
        try
        {
            _dockService.CreateDock(ds);
            return Created();
        }
        catch (ModelInvalidException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public IActionResult UpdateDockingSpot([FromBody] DockingSpotDto ds)
    {
        try
        {
            _dockService.UpdateDock(ds);
            return Ok();
        }
        catch (ModelInvalidException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDockingSpot([FromRoute] int id)
    {
        try
        {
            _dockService.DeleteDock(id);
            return Ok();
        }
        catch (ModelInvalidException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetDockingSpot([FromRoute] int id)
    {
        var ds = _dockService.GetDockById(id);
        if (ds == null)
            return NotFound();
        return Ok(DockingSpotReturnDto.FromModel(ds));
    }
}