using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category?> CreateAsync(Category category);
        Task<Category?> UpdateAsync(Category category);
        Task<bool> DeleteAsync(Guid id);
        Task<(List<Category> Categories, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null);
    }
}