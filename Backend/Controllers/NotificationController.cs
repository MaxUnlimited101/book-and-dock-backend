using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        // private readonly INotificationService _notificationService;

        // public NotificationController(INotificationService notificationService)
        // {
        //     _notificationService = notificationService;
        // }

        // [HttpGet]
        // public IActionResult GetAllNotifications()
        // {

        // }

        // [HttpGet("{id}")]
        // public IActionResult GetNotification(int id)
        // {

        // }

        // [HttpPost]
        // public IActionResult CreateNotification([FromBody] string notification)
        // {
            
        // }

        // [HttpPut("{id}")]
        // public IActionResult UpdateNotification(int id, [FromBody] string updatedNotification)
        // {
            
        // }

        // [HttpDelete("{id}")]
        // public IActionResult DeleteNotification(int id)
        // {
            
        // }
    }
}