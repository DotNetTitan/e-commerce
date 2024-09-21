using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderId = Guid.NewGuid();
            OrderItems = new List<OrderItem>();
            Status = OrderStatus.Pending;
        }

        public Guid OrderId { get; set; }
        public required DateTime OrderDate { get; set; }
        public required Guid CustomerId { get; set; }
        public decimal TotalAmount => OrderItems.Sum(item => item.TotalPrice);
        public OrderStatus Status { get; private set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; private set; }
        public required Address ShippingAddress { get; set; }

        // Add an order item
        public void AddOrderItem(OrderItem item)
        {
            OrderItems.Add(item);
        }

        // Remove an order item
        public void RemoveOrderItem(OrderItem item)
        {
            OrderItems.Remove(item);
        }

        // Update order status
        public void UpdateStatus(OrderStatus status)
        {
            Status = status;
        }

        // Check if the order is empty
        public bool IsEmpty() => !OrderItems.Any();

        // Get the number of items in the order
        public int ItemCount => OrderItems.Sum(item => item.Quantity);

        // Get the number of unique products in the order
        public int UniqueItemCount => OrderItems.Count;
    }
}