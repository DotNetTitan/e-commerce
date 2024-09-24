using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(Guid customerId)
        {
            return await _context.Customers
                .Include(c => c.CustomerAddress)
                .Include(c => c.ShoppingCart)
                .Include(c => c.Orders)
                .Include(c => c.Reviews)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}