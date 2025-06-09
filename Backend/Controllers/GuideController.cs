using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;
using Backend.Interfaces;
using Backend.DTO;
using Backend.Exceptions;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/guide")]
    [Authorize]
    public class GuideController : ControllerBase
    {
        private readonly IGuideService _guideService;

        public GuideController(IGuideService guideService)
        {
            _guideService = guideService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_guideService.GetAllGuides());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var guide = _guideService.GetGuideById(id);
            if (guide == null)
            {
                return NotFound();
            }
            return Ok(guide);
        }

        [HttpPost]
        public IActionResult Create([FromBody] GuideDto guide)
        {
            try
            {
                _guideService.CreateGuide(guide);
                return Created();
            }
            catch (ModelInvalidException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] GuideDto updatedGuide)
        {
            try
            {
                _guideService.UpdateGuide(id, updatedGuide);
                return Ok();
            }
            catch (ModelInvalidException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _guideService.DeleteGuide(id);
            return NoContent();
        }
    }
}