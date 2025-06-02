using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.DTO;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backend.Tests.Controllers;

public class ReviewControllerTests
{
    private readonly ReviewController _controller;
    private readonly Mock<IReviewService> _reviewServiceMock;

    public ReviewControllerTests()
    {
        _reviewServiceMock = new Mock<IReviewService>();
        _controller = new ReviewController(_reviewServiceMock.Object);
    }

    [Fact]
    public async Task GetReviewAsync_ReturnsReview_WhenExists()
    {
        var dto = new ReviewDTO(1, 1, 1, 4.5, "Great!", DateTime.UtcNow, null);
        _reviewServiceMock.Setup(s => s.GetReviewByIdAsync(1)).ReturnsAsync(dto);

        var result = await _controller.GetReviewAsync(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<ReviewDTO>(ok.Value);
        Assert.Equal(1, returned.Id);
    }


    [Fact]
    public async Task GetAllReviewsAsync_ReturnsList()
    {
        var reviews = new List<ReviewDTO> {
            new ReviewDTO(1, 1, 1, 5.0, "Awesome", DateTime.UtcNow, null)
        };
        _reviewServiceMock.Setup(s => s.GetAllReviewsAsync()).ReturnsAsync(reviews);

        var result = await _controller.GetAllReviewsAsync();

        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsAssignableFrom<IEnumerable<ReviewDTO>>(ok.Value);
        Assert.Single(list);
    }

    [Fact]
    public async Task CreateReviewAsync_ReturnsCreated_WhenValid()
    {
        var createDto = new CreateReviewDTO(1, 1, 5, "Nice");
        var created = new ReviewDTO(1, 1, 1, 5, "Nice", DateTime.UtcNow, null);

        _reviewServiceMock.Setup(s => s.CreateReviewAsync(createDto)).ReturnsAsync(created);

        var result = await _controller.CreateReviewAsync(createDto);

        var createdResult = Assert.IsType<CreatedResult>(result);
        var returned = Assert.IsType<ReviewDTO>(createdResult.Value);
        Assert.Equal(1, returned.Id);
    }

    [Fact]
    public async Task UpdateReviewAsync_ReturnsOk_WhenSuccessful()
    {
        var updateDto = new UpdateReviewDTO(1, 4, "Updated");
        _reviewServiceMock.Setup(s => s.UpdateReviewAsync(1, updateDto)).ReturnsAsync(true);

        var result = await _controller.UpdateReviewAsync(updateDto);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateReviewAsync_ReturnsNotFound_WhenFails()
    {
        var updateDto = new UpdateReviewDTO(1, 4, "Updated");
        _reviewServiceMock.Setup(s => s.UpdateReviewAsync(1, updateDto)).ReturnsAsync(false);

        var result = await _controller.UpdateReviewAsync(updateDto);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Review not found or update failed.", ((dynamic)notFound.Value).Message);
    }

    

    [Fact]
    public async Task GetDockingSpotReviewsAsync_ReturnsList()
    {
        var list = new List<ReviewDTO> {
            new ReviewDTO(1, 1, 2, 3.5, "Ok", DateTime.UtcNow, null)
        };
        _reviewServiceMock.Setup(s => s.GetDockingSpotReviews(2)).ReturnsAsync(list);

        var result = await _controller.GetDockingSpotReviewsAsync(2);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Single((IEnumerable<ReviewDTO>)ok.Value);
    }

    [Fact]
    public async Task GetUserReviews_ReturnsList()
    {
        var list = new List<ReviewDTO> {
            new ReviewDTO(1, 2, 1, 4.0, "Good", DateTime.UtcNow, null)
        };
        _reviewServiceMock.Setup(s => s.GetUserReviews(2)).ReturnsAsync(list);

        var result = await _controller.GetUserReviews(2);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Single((IEnumerable<ReviewDTO>)ok.Value);
    }
}
