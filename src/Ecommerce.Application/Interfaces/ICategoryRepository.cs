using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid categoryId);
        Task<Category?> CreateAsync(Category category);
        Task<Category?> UpdateAsync(Category category);
        Task<bool> DeleteAsync(Category category);
        Task<(List<Category> Categories, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null);
        Task<Category?> GetByNameAsync(string name);
    }
}