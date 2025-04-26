using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;
using Backend.Interfaces;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GuideController : ControllerBase
    {
        // private readonly IGuideService _guideService;

        // public GuideController(IGuideService guideService)
        // {
        //     _guideService = guideService;
        // }

        // [HttpGet]
        // public IActionResult GetAll()
        // {
            
        // }

        // [HttpGet("{id}")]
        // public IActionResult GetById(int id)
        // {

        // }

        // [HttpPost]
        // public IActionResult Create([FromBody] Guide guide)
        // {
            
        // }

        // [HttpPut("{id}")]
        // public IActionResult Update(int id, [FromBody] Guide updatedGuide)
        // {
            
        // }

        // [HttpDelete("{id}")]
        // public IActionResult Delete(int id)
        // {
            
        // }
    }
}