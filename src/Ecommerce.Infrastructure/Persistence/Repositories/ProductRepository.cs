﻿using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Repositories
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
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == id);
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
                .Include(p => p.Images)
                .AsQueryable();

            query = ApplyFilters(query, searchTerm, categoryId);

            var totalCount = await query.CountAsync();

            // Ensure pageNumber is valid
            pageNumber = Math.Max(1, pageNumber);
            var skipAmount = (pageNumber - 1) * pageSize;

            // Adjust skip amount if it's greater than total count
            if (skipAmount >= totalCount)
            {
                skipAmount = 0;
                pageNumber = 1;
            }

            var products = await query
                .OrderBy(p => p.Name)
                .Skip(skipAmount)
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
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }
    }
}
