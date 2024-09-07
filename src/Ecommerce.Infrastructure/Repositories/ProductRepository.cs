using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Infrastructure.Extensions;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                await _context.Entry(product)
                    .Reference(p => p.Category)
                    .LoadAsync();
            }

            return product;
        }

        public async Task<Product?> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<(List<Product> Products, int TotalCount)> GetProductsAsync(int pageNumber, int pageSize, string? searchTerm = null, Guid? categoryId = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            query = ApplyFilters(query, searchTerm, categoryId);

            var totalCount = await query.CountAsync();

            var products = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        private static IQueryable<Product> ApplyFilters(IQueryable<Product> query, string? searchTerm, Guid? categoryId)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            return query;
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products
                .FirstOrDefaultIgnoreCaseAsync(p => p.Name, name);
        }
    }
}