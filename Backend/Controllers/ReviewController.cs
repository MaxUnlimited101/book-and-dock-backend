using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DTO;
using Backend.DTO.Review;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }
    
    // [HttpPost("ds/{id}")]
    // public ActionResult<StatusReturnDto> CreateReview([FromRoute] int id, [FromBody] CreateReviewDTO reviewDto)
    // {
        
    // }

    // [HttpGet("ds/{id}")]
    // public IActionResult GetDockingSpotReviews([FromRoute] int id)
    // {
    // }
}