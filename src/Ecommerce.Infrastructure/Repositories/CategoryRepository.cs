using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetByIdAsync(Guid categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public async Task<Category?> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<(List<Category> Categories, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var categories = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (categories, totalCount);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}