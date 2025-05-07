using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetNotifications()
        {
            var notifications = await _notificationService.GetNotificationsAsync();
            return Ok(notifications.Select(NotificationDTO.FromModel));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDTO>> GetNotificationById(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(NotificationDTO.FromModel(notification));
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationDTO notificationDto)
        {
            try
            {
                var id = await _notificationService.CreateNotificationAsync(notificationDto);
                return CreatedAtAction(nameof(GetNotificationById), new { id });
            }
            catch (ModelInvalidException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] NotificationDTO notificationDto)
        {
            try
            {
                notificationDto = notificationDto.WithId(id);
                await _notificationService.UpdateNotificationAsync(notificationDto);
                return NoContent();
            }
            catch (ModelInvalidException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                await _notificationService.DeleteNotificationAsync(id);
                return NoContent();
            }
            catch (ModelInvalidException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}