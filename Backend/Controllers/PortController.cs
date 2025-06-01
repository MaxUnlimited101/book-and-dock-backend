using Backend.DTO.Port;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PortController : ControllerBase
    {
        private readonly IPortService _portService;
        private readonly IPortRepository _portRepository;
        private readonly IBookingRepository _bookingRepository;

        public PortController(IPortService portService, IPortRepository portRepository, IBookingRepository bookingRepository)
        {
            _portService = portService;
            _portRepository = portRepository;
            _bookingRepository = bookingRepository;
        }

        [HttpPost]
        public IActionResult CreatePort([FromBody] PortDto portDto)
        {
            var newPortId = _portService.Create(portDto);
            return CreatedAtAction(nameof(GetPortById), new { portId = newPortId }, new { id = newPortId });
        }

        [HttpPut("{portId}")]
        public IActionResult UpdatePort(int portId, [FromBody] PortDto portDto)
        {
            var existingPort = _portRepository.GetById(portId);
            if (existingPort == null) return NotFound();

            existingPort.Name = portDto.Name;
            existingPort.Description = portDto.Description;
            existingPort.IsApproved = portDto.IsApproved;

            _portRepository.Update(existingPort);
            return Ok(new { message = "Port updated." });
        }

        [HttpGet("dock-owner/bookings")]
        public IActionResult GetDockOwnerBookings()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0) return Unauthorized();

            var bookings = _bookingRepository.GetBookingsByDockOwnerId(userId);
            return Ok(bookings);
        }

        [HttpGet("{portId}")]
        public IActionResult GetPortById(int portId)
        {
            var port = _portRepository.GetById(portId);
            return port != null ? Ok(port) : NotFound();
        }
    }
}
