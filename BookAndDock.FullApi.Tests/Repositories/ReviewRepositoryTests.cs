using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests.Repositories
{
    public class ReviewRepositoryTests
    {
        private readonly DbContextOptions<BookAndDockContext> _options;

        public ReviewRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private Review CreateSampleReview(int id = 0)
        {
            return new Review
            {
                Id = id,
                UserId = 1,
                DockId = 2,
                Rating = 4.5,
                Content = "Great place!",
                CreatedAt = DateTime.UtcNow
            };
        }

        [Fact]
        public void CreateReview_AddsToDatabase()
        {
            using var context = new BookAndDockContext(_options);
            var repo = new ReviewRepository(context);
            var review = CreateSampleReview();

            var id = repo.CreateReview(review);

            Assert.True(id > 0);
            Assert.NotNull(context.Reviews.Find(id));
        }

        [Fact]
        public async Task CreateReviewAsync_AddsToDatabase()
        {
            using var context = new BookAndDockContext(_options);
            var repo = new ReviewRepository(context);
            var review = CreateSampleReview();

            var id = await repo.CreateReviewAsync(review);

            Assert.True(id > 0);
            Assert.NotNull(await context.Reviews.FindAsync(id));
        }

        [Fact]
        public void GetReviewById_ReturnsCorrectReview()
        {
            using var context = new BookAndDockContext(_options);
            var review = CreateSampleReview(10);
            context.Reviews.Add(review);
            context.SaveChanges();

            var repo = new ReviewRepository(context);
            var result = repo.GetReviewById(10);

            Assert.NotNull(result);
            Assert.Equal(10, result!.Id);
        }

        [Fact]
        public async Task GetReviewByIdAsync_ReturnsCorrectReview()
        {
            using var context = new BookAndDockContext(_options);
            var review = CreateSampleReview(11);
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            var repo = new ReviewRepository(context);
            var result = await repo.GetReviewByIdAsync(11);

            Assert.NotNull(result);
            Assert.Equal(11, result!.Id);
        }

        [Fact]
        public void GetAllReviews_ReturnsAll()
        {
            using var context = new BookAndDockContext(_options);
            context.Reviews.AddRange(
                CreateSampleReview(1),
                CreateSampleReview(2)
            );
            context.SaveChanges();

            var repo = new ReviewRepository(context);
            var result = repo.GetAllReviews();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllReviewsAsync_ReturnsAll()
        {
            using var context = new BookAndDockContext(_options);
            context.Reviews.AddRange(
                CreateSampleReview(3),
                CreateSampleReview(4)
            );
            await context.SaveChangesAsync();

            var repo = new ReviewRepository(context);
            var result = await repo.GetAllReviewsAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void UpdateReview_UpdatesReview()
        {
            using var context = new BookAndDockContext(_options);
            var review = CreateSampleReview(5);
            context.Reviews.Add(review);
            context.SaveChanges();

            review.Content = "Updated!";
            var repo = new ReviewRepository(context);
            var updated = repo.UpdateReview(5, review);

            Assert.True(updated);
            Assert.Equal("Updated!", context.Reviews.Find(5)!.Content);
        }

        [Fact]
        public async Task UpdateReviewAsync_UpdatesReview()
        {
            using var context = new BookAndDockContext(_options);
            var review = CreateSampleReview(6);
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            review.Content = "Async Update!";
            var repo = new ReviewRepository(context);
            var result = await repo.UpdateReviewAsync(6, review);

            Assert.True(result);
            Assert.Equal("Async Update!", (await context.Reviews.FindAsync(6))!.Content);
        }

        [Fact]
        public void DeleteReview_RemovesReview()
        {
            using var context = new BookAndDockContext(_options);
            var review = CreateSampleReview(7);
            context.Reviews.Add(review);
            context.SaveChanges();

            var repo = new ReviewRepository(context);
            repo.DeleteReview(7);

            Assert.Null(context.Reviews.Find(7));
        }

        [Fact]
        public async Task DeleteReviewAsync_RemovesReview()
        {
            using var context = new BookAndDockContext(_options);
            var review = CreateSampleReview(8);
            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            var repo = new ReviewRepository(context);
            await repo.DeleteReviewAsync(8);

            Assert.Null(await context.Reviews.FindAsync(8));
        }

        [Fact]
        public async Task GetDockingSpotReviewsAsync_ReturnsReviews()
        {
            using var context = new BookAndDockContext(_options);
            context.Reviews.AddRange(
                new Review { Id = 9, DockId = 101, UserId = 1, Rating = 4, Content = "Good", CreatedAt = DateTime.UtcNow },
                new Review { Id = 10, DockId = 101, UserId = 2, Rating = 5, Content = "Great", CreatedAt = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();

            var repo = new ReviewRepository(context);
            var result = await repo.GetDockingSpotReviewsAsync(101);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetUserReviewsAsync_ReturnsReviews()
        {
            using var context = new BookAndDockContext(_options);
            context.Reviews.AddRange(
                new Review { Id = 11, DockId = 201, UserId = 5, Rating = 3, Content = "Okay", CreatedAt = DateTime.UtcNow },
                new Review { Id = 12, DockId = 202, UserId = 5, Rating = 4, Content = "Nice", CreatedAt = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();

            var repo = new ReviewRepository(context);
            var result = await repo.GetUserReviewsAsync(5);

            Assert.Equal(2, result.Count());
        }
    }
}
