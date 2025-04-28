using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Moq;
using Xunit;

namespace Backend.Tests.Services
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewRepository> _reviewRepoMock;
        private readonly ReviewService _svc;

        public ReviewServiceTests()
        {
            _reviewRepoMock = new Mock<IReviewRepository>();
            _svc = new ReviewService(_reviewRepoMock.Object);
        }

        [Fact]
        public async Task CreateReviewAsync_ReturnsMappedDto()
        {
            // Arrange
            var dto = new CreateReviewDTO(UserId: 1, DockId: 2, Rating: 5, Content: "Nice!");
            var reviewEntity = new Review
            {
                Id = 10,
                UserId = dto.UserId,
                DockId = dto.DockId,
                Rating = dto.Rating,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };
            _reviewRepoMock.Setup(r => r.CreateReviewAsync(It.IsAny<Review>())).ReturnsAsync(reviewEntity.Id);
            _reviewRepoMock.Setup(r => r.GetReviewByIdAsync(reviewEntity.Id)).ReturnsAsync(reviewEntity);

            // Act
            var result = await _svc.CreateReviewAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reviewEntity.Id, result.Id);
            Assert.Equal(dto.Content, result.Content);
        }

        [Fact]
        public async Task GetReviewByIdAsync_NotFound_ReturnsNull()
        {
            // Arrange
            _reviewRepoMock.Setup(r => r.GetReviewByIdAsync(1)).ReturnsAsync((Review?)null);

            // Act
            var result = await _svc.GetReviewByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetReviewByIdAsync_Found_ReturnsDto()
        {
            // Arrange
            var review = new Review
            {
                Id = 2,
                UserId = 1,
                DockId = 3,
                Rating = 4,
                Content = "Good!",
                CreatedAt = DateTime.UtcNow
            };
            _reviewRepoMock.Setup(r => r.GetReviewByIdAsync(2)).ReturnsAsync(review);

            // Act
            var result = await _svc.GetReviewByIdAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(review.Id, result!.Id);
        }

        [Fact]
        public async Task GetAllReviewsAsync_ReturnsMappedDtos()
        {
            // Arrange
            var list = new List<Review>
            {
                new Review { Id = 1, UserId = 1, DockId = 2, Rating = 5, Content = "A", CreatedAt = DateTime.UtcNow },
                new Review { Id = 2, UserId = 2, DockId = 3, Rating = 4, Content = "B", CreatedAt = DateTime.UtcNow }
            };
            _reviewRepoMock.Setup(r => r.GetAllReviewsAsync()).ReturnsAsync(list);

            // Act
            var result = await _svc.GetAllReviewsAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateReviewAsync_NotFound_ReturnsFalse()
        {
            // Arrange
            var dto = new UpdateReviewDTO(Id: 1, Rating: 5, Content: "Update");
            _reviewRepoMock.Setup(r => r.GetReviewByIdAsync(dto.Id)).ReturnsAsync((Review?)null);

            // Act
            var result = await _svc.UpdateReviewAsync(dto.Id, dto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateReviewAsync_Found_UpdatesAndReturnsTrue()
        {
            // Arrange
            var existing = new Review
            {
                Id = 5,
                UserId = 2,
                DockId = 3,
                Rating = 2,
                Content = "Old",
                CreatedAt = DateTime.UtcNow
            };
            var dto = new UpdateReviewDTO(Id: 5, Rating: 4, Content: "Updated");

            _reviewRepoMock.Setup(r => r.GetReviewByIdAsync(dto.Id)).ReturnsAsync(existing);
            _reviewRepoMock.Setup(r => r.UpdateReviewAsync(dto.Id, It.IsAny<Review>())).ReturnsAsync(true);

            // Act
            var result = await _svc.UpdateReviewAsync(dto.Id, dto);

            // Assert
            Assert.True(result);
            _reviewRepoMock.Verify(r => r.UpdateReviewAsync(dto.Id, It.Is<Review>(rev =>
                rev.Rating == dto.Rating && rev.Content == dto.Content
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteReviewAsync_CallsRepository()
        {
            // Arrange
            _reviewRepoMock.Setup(r => r.DeleteReviewAsync(7)).Returns(Task.CompletedTask);

            // Act
            await _svc.DeleteReviewAsync(7);

            // Assert
            _reviewRepoMock.Verify(r => r.DeleteReviewAsync(7), Times.Once);
        }

        [Fact]
        public async Task GetDockingSpotReviews_ReturnsMappedDtos()
        {
            // Arrange
            var list = new List<Review>
            {
                new Review { Id = 1, DockId = 5, UserId = 1, Rating = 5, Content = "A", CreatedAt = DateTime.UtcNow }
            };
            _reviewRepoMock.Setup(r => r.GetDockingSpotReviewsAsync(5)).ReturnsAsync(list);

            // Act
            var result = await _svc.GetDockingSpotReviews(5);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GetUserReviews_ReturnsMappedDtos()
        {
            // Arrange
            var list = new List<Review>
            {
                new Review { Id = 1, DockId = 2, UserId = 3, Rating = 3, Content = "Good", CreatedAt = DateTime.UtcNow }
            };
            _reviewRepoMock.Setup(r => r.GetUserReviewsAsync(3)).ReturnsAsync(list);

            // Act
            var result = await _svc.GetUserReviews(3);

            // Assert
            Assert.Single(result);
        }
    }
}
