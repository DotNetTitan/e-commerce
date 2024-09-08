using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart?> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.ShoppingCarts
                .Include(sc => sc.ShoppingCartItems)
                .ThenInclude(sci => sci.Product)
                .FirstOrDefaultAsync(sc => sc.CustomerId == customerId);
        }

        public async Task<ShoppingCart?> UpdateAsync(ShoppingCart shoppingCart)
        {
            _context.Entry(shoppingCart).State = EntityState.Modified;
            foreach (var item in shoppingCart.ShoppingCartItems)
            {
                _context.Entry(item).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return shoppingCart;
        }
    }
}