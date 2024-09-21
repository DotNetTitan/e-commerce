using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(Guid orderId);
        Task<bool> OrderExistsAsync(Guid orderId);
        Task<Order?> GetOrderByOrderIdAsync(Guid orderId);
        Task<List<Order>> GetOrdersByCustomerIdAsync(Guid customerId);
    }
}