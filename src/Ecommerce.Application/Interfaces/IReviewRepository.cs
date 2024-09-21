using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> GetByIdAsync(Guid reviewId);
        Task<Review> CreateAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task<bool> DeleteAsync(Review review);
        Task<List<Review>> GetReviewsByCustomerIdAsync(Guid customerId);
        Task<(List<Review> Reviews, int TotalCount)> GetReviewsByProductIdAsync(Guid productId, int pageNumber, int pageSize);
    }
}