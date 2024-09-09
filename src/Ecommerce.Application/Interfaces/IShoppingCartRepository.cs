using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart?> GetByCustomerIdAsync(Guid customerId);
        Task<ShoppingCart?> UpdateAsync(ShoppingCart shoppingCart);
        Task<bool> ClearAsync(Guid customerId);
        Task<ShoppingCart> CreateAsync(ShoppingCart shoppingCart);
    }
}