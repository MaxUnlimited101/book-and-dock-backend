using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PortController : ControllerBase
    {
        // private readonly IPortService _portService;

        // public PortController(IPortService portService)
        // {
        //     _portService = portService;
        // }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<PortDto>>> GetAllPorts()
        // {
        //     var ports = await _portService.GetAllPortsAsync();
        //     return Ok(ports);
        // }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<PortDto>> GetPortById(int id)
        // {
        //     var port = await _portService.GetPortByIdAsync(id);
        //     if (port == null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(port);
        // }

        // [HttpPost]
        // public async Task<ActionResult> CreatePort([FromBody] PortDto portDto)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     await _portService.CreatePortAsync(portDto);
        //     return CreatedAtAction(nameof(GetPortById), new { id = portDto.Id }, portDto);
        // }

        // [HttpPut("{id}")]
        // public async Task<ActionResult> UpdatePort(int id, [FromBody] PortDto portDto)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     var updated = await _portService.UpdatePortAsync(id, portDto);
        //     if (!updated)
        //     {
        //         return NotFound();
        //     }

        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult> DeletePort(int id)
        // {
        //     var deleted = await _portService.DeletePortAsync(id);
        //     if (!deleted)
        //     {
        //         return NotFound();
        //     }

        //     return NoContent();
        // }
    }
}