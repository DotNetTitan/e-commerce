using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product?> CreateAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Product product);
        Task<(List<Product> Products, int TotalCount)> GetProductsAsync(int pageNumber, int pageSize, string? searchTerm = null, Guid? categoryId = null);
        Task<Product?> GetByNameAsync(string name);
        Task<List<Product>> GetAllAsync();
    }
}