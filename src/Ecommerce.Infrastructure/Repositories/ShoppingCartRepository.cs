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

        public async Task<bool> ClearAsync(Guid customerId)
        {
            var itemsDeleted = await _context.ShoppingCartItems
                .Where(sci => sci.ShoppingCart != null && sci.ShoppingCart.CustomerId == customerId)
                .ExecuteDeleteAsync();

            if (itemsDeleted > 0)
            {
                // If items were deleted, we know the cart existed
                return true;
            }

            // If no items were deleted, check if the cart exists
            var cartExists = await _context.ShoppingCarts
                .AnyAsync(sc => sc.CustomerId == customerId);

            return cartExists;
        }

        public async Task<ShoppingCart> CreateAsync(ShoppingCart shoppingCart)
        {
            await _context.ShoppingCarts.AddAsync(shoppingCart);
            await _context.SaveChangesAsync();
            return shoppingCart;
        }
    }
}