using Backend.Models;

namespace Backend.Interfaces
{
    public interface IReviewRepository
    {
        // CREATE
        int CreateReview(Review review);
        Task<int> CreateReviewAsync(Review review);

        // READ
        Review? GetReviewById(int id);
        Task<Review?> GetReviewByIdAsync(int id);
        List<Review> GetAllReviews();
        Task<List<Review>> GetAllReviewsAsync();

        // UPDATE
        bool UpdateReview(int id, Review updatedReview);
        Task<bool> UpdateReviewAsync(int id, Review updatedReview);

        // DELETE
        void DeleteReview(int id);
        Task DeleteReviewAsync(int id);
    }
}