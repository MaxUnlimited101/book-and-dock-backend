using Backend.DTO;
using Backend.Exceptions;
using Backend.Services;
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
        private readonly LocationService _locationService;

        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await _locationService.GetAllLocationsAsync();
            if (locations == null || locations.Count() == 0)
            {
                return NotFound("No locations found.");
            }
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound($"Location with ID {id} not found.");
            }
            return Ok(location);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] LocationDto location)
        {
            try
            {
                await _locationService.CreateLocationAsync(location);
                return Created();
            }
            catch (ModelInvalidException ex)
            {
                return BadRequest($"Error creating location: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationDto updatedLocation)
        {
            try
            {
                var location = await _locationService.GetLocationByIdAsync(updatedLocation.Id);
                if (location == null)
                {
                    return NotFound($"Location with ID {updatedLocation.Id} not found.");
                }
                await _locationService.UpdateLocationAsync(updatedLocation);
                return Ok();
            }
            catch (ModelInvalidException ex)
            {
                return BadRequest($"Error updating location: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                await _locationService.DeleteLocationAsync(id);
                return Ok();
            }
            catch (ModelInvalidException ex)
            {
                return BadRequest($"Error deleting location: {ex.Message}");
            }
        }
    }
}