using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LocationController : ControllerBase
    {
        // private readonly LocationService _locationService;

        // public LocationController(LocationService locationService)
        // {
        //     _locationService = locationService;
        // }

        // [HttpGet]
        // public IActionResult GetAllLocations()
        // {

        // }

        // [HttpGet("{id}")]
        // public IActionResult GetLocationById(int id)
        // {
            
        // }

        // [HttpPost]
        // public IActionResult CreateLocation([FromBody] Location location)
        // {
            
        // }

        // [HttpPut("{id}")]
        // public IActionResult UpdateLocation(int id, [FromBody] Location updatedLocation)
        // {
            
        // }

        // [HttpDelete("{id}")]
        // public IActionResult DeleteLocation(int id)
        // {
            
        // }
    }
}