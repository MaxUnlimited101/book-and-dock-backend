using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BookAndDockContext _context;

        public ReviewRepository(BookAndDockContext context)
        {
            _context = context;
        }

        public int CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return review.Id;
        }

        public async Task<int> CreateReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review.Id;
        }

        public Review? GetReviewById(int id)
        {
            return _context.Reviews.Find(id);
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public List<Review> GetAllReviews()
        {
            return _context.Reviews.ToList();
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public bool UpdateReview(int id, Review updatedReview)
        {
            var review = _context.Reviews.Find(id);
            if (review == null)
            {
                return false;
            }
            _context.Entry(review).CurrentValues.SetValues(updatedReview);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> UpdateReviewAsync(int id, Review updatedReview)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return false;
            }
            _context.Entry(review).CurrentValues.SetValues(updatedReview);
            await _context.SaveChangesAsync();
            return true;
        }

        public void DeleteReview(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }

        public Task<IEnumerable<Review>> GetDockingSpotReviewsAsync(int dockingSpotId)
        {
            return Task.FromResult(_context.Reviews
                .Where(r => r.DockId == dockingSpotId)
                .AsEnumerable());
        }

        public Task<IEnumerable<Review>> GetUserReviewsAsync(int userId)
        {
            return Task.FromResult(_context.Reviews
                .Where(r => r.UserId == userId)
                .AsEnumerable());
        }
    }
}