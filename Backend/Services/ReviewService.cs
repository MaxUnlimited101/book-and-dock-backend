using Backend.DTO.Review;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<ReviewDTO> CreateReviewAsync(CreateReviewDTO reviewDto)
        {
            // Map DTO to Review entity.
            var review = new Review
            {
                UserId = reviewDto.UserId,
                DockId = reviewDto.DockId,
                Rating = reviewDto.Rating,
                Content = reviewDto.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            // Create review in repository.
            var id = await _reviewRepository.CreateReviewAsync(review);
            // Optionally, retrieve the created review.
            var createdReview = await _reviewRepository.GetReviewByIdAsync(id);
            return MapToDto(createdReview!);
        }

        public async Task<ReviewDTO?> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null)
            {
                return null;
            }
            return MapToDto(review);
        }

        public async Task<List<ReviewDTO>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllReviewsAsync();
            return reviews.Select(r => MapToDto(r)).ToList();
        }

        public async Task<bool> UpdateReviewAsync(int id, UpdateReviewDTO reviewDto)
        {
            // Retrieve the existing review.
            var existingReview = await _reviewRepository.GetReviewByIdAsync(id);
            if (existingReview == null)
            {
                return false;
            }

            // Update properties.
            existingReview.Rating = reviewDto.Rating;
            existingReview.Comment = reviewDto.Content;

            // Update review in repository.
            return await _reviewRepository.UpdateReviewAsync(id, existingReview);
        }

        public Task DeleteReviewAsync(int id)
        {
            return _reviewRepository.DeleteReviewAsync(id);
        }

    }
}