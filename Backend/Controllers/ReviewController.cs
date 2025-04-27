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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReviewAsync([FromRoute] int id)
    {
        var review = await _reviewService.GetReviewByIdAsync(id);
        if (review == null)
        {
            return NotFound(new { Message = "Review not found." });
        }
        return Ok(review);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReviewsAsync()
    {
        var reviews = await _reviewService.GetAllReviewsAsync();
        if (reviews == null || !reviews.Any())
        {
            return NotFound(new { Message = "No reviews found." });
        }
        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReviewAsync([FromBody] CreateReviewDTO reviewDto)
    {
        var createdReview = await _reviewService.CreateReviewAsync(reviewDto);
        if (createdReview == null)
        {
            return BadRequest(new { Message = "Failed to create review." });
        }
        return Created($"/api/review/{createdReview.Id}", createdReview);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateReviewAsync([FromBody] UpdateReviewDTO reviewDto)
    {
        var updated = await _reviewService.UpdateReviewAsync(reviewDto.Id, reviewDto);
        if (!updated)
        {
            return NotFound(new { Message = "Review not found or update failed." });
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReviewAsync([FromRoute] int id)
    {
        await _reviewService.DeleteReviewAsync(id);
        return NoContent();
    }

    [HttpGet("ds/{dsId}")]
    public async Task<IActionResult> GetDockingSpotReviewsAsync([FromRoute] int dsId)
    {
        var reviews = await _reviewService.GetDockingSpotReviews(dsId);
        if (reviews == null || !reviews.Any())
        {
            return NotFound(new { Message = "No reviews found for this docking spot." });
        }
        return Ok(reviews);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserReviews([FromRoute] int userId)
    {
        var reviews = await _reviewService.GetUserReviews(userId);
        if (reviews == null || !reviews.Any())
        {
            return NotFound(new { Message = "No reviews found for this user." });
        }
        return Ok(reviews);
    }
}