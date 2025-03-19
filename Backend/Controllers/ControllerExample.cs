using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/example")]
public class ControllerExample : ControllerBase
{
    [HttpGet]
    public ActionResult<string> GetSomeString()
    {
        return Ok("Some string");
    }
}