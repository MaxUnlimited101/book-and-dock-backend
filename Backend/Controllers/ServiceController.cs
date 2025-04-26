using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServiceController : ControllerBase
    {
        // private readonly IServiceService _serviceService;

        // public ServiceController(IServiceService service)
        // {
        //     _service = service;
        // }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAll()
        // {
        //     var services = await _service.GetAllAsync();
        //     return Ok(services);
        // }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<ServiceDto>> GetById(int id)
        // {
        //     var service = await _service.GetByIdAsync(id);
        //     if (service == null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(service);
        // }

        // [HttpPost]
        // public async Task<ActionResult<ServiceDto>> Create([FromBody] ServiceDto serviceDto)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     var createdService = await _service.CreateAsync(serviceDto);
        //     return CreatedAtAction(nameof(GetById), new { id = createdService.Id }, createdService);
        // }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> Update(int id, [FromBody] ServiceDto serviceDto)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     var updated = await _service.UpdateAsync(id, serviceDto);
        //     if (!updated)
        //     {
        //         return NotFound();
        //     }

        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     var deleted = await _service.DeleteAsync(id);
        //     if (!deleted)
        //     {
        //         return NotFound();
        //     }

        //     return NoContent();
        // }
    }
}