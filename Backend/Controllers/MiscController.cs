using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
public class MiscController : ControllerBase
{
    [HttpGet("api/health")]
    public IActionResult HealthCheck()
    {
        return Ok(new { status = "healthy" });
    }
}