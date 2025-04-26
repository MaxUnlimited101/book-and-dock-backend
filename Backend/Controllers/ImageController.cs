using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImageController : ControllerBase
    {
        // private readonly ImageService _imageService;

        // public ImageController(ImageService imageService)
        // {
        //     _imageService = imageService;
        // }

        // [HttpGet]
        // public IActionResult GetAllImages()
        // {
            
        // }

        // [HttpGet("{id}")]
        // public IActionResult GetImage(int id)
        // {
            
        // }

        // [HttpPost]
        // public IActionResult AddImage([FromBody] string imageUrl)
        // {
            
        // }

        // [HttpPut("{id}")]
        // public IActionResult UpdateImage(int id, [FromBody] string newImageUrl)
        // {
            
        // }

        // [HttpDelete("{id}")]
        // public IActionResult DeleteImage(int id)
        // {
            
        // }
    }
}