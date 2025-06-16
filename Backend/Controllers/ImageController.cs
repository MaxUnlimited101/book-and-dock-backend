using Backend.DTO.Image;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadImageDto dto)
        {
            var result = await _imageService.UploadImageAsync(dto);
            return Ok(result);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadImageById(int id)
        {
            var (stream, contentType) = await _imageService.GetImageStreamByIdAsync(id);
            if (stream == null)
                return NotFound("Image not found in database or S3");

            contentType ??= "application/octet-stream"; // fallback
            return File(stream, contentType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var success = await _imageService.DeleteImageByIdAsync(id);
            if (!success)
                return NotFound("Image not found or already deleted.");

            return Ok("Image deleted successfully.");
        }
    }

}