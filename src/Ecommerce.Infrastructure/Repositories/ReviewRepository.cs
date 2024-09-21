using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review?> GetByIdAsync(Guid reviewId)
        {
            return await _context.Reviews.FindAsync(reviewId);
        }

        public async Task<Review> CreateAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteAsync(Review review)
        {
            _context.Reviews.Remove(review);
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<(List<Review> Reviews, int TotalCount)> GetReviewsByProductIdAsync(Guid productId, int pageNumber, int pageSize)
        {
            var query = _context.Reviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt);

            var totalCount = await query.CountAsync();

            var reviews = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (reviews, totalCount);
        }

        public async Task<List<Review>> GetReviewsByCustomerIdAsync(Guid customerId)
        {
            return await _context.Reviews
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}