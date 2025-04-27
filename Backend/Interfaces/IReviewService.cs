using Backend.DTO;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IReviewService
    {
        // Creates a new review and returns the created review as DTO.
        Task<ReviewDTO> CreateReviewAsync(CreateReviewDTO reviewDto);

        // Retrieves a review by its id as DTO.
        Task<ReviewDTO?> GetReviewByIdAsync(int id);

        // Retrieves all reviews as a list of DTOs.
        Task<List<ReviewDTO>> GetAllReviewsAsync();

        // Updates a review identified by id using update DTO.
        // Returns true if update is successful, false otherwise.
        Task<bool> UpdateReviewAsync(int id, UpdateReviewDTO reviewDto);

        // Deletes a review by its id.
        Task DeleteReviewAsync(int id);

        // Retrieves all reviews for a specific DockingSpot by its id as a list of DTOs.
        Task<IEnumerable<ReviewDTO>> GetDockingSpotReviews(int dockingSpotId);
        
        // Retrieves all reviews for a specific User by its id as a list of DTOs.
        Task<IEnumerable<ReviewDTO>> GetUserReviews(int userId);
    }
}