using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents an order in the e-commerce system.
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the order was placed.
        /// </summary>
        public required DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier associated with the order.
        /// </summary>
        public required Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the status of the order.
        /// </summary>
        public OrderStatus Status { get; private set; }

        /// <summary>
        /// Gets or sets the customer associated with the order.
        /// </summary>
        public Customer? Customer { get; set; }

        /// <summary>
        /// Gets or sets the collection of order items.
        /// </summary>
        public ICollection<OrderItem> OrderItems { get; private set; }

        /// <summary>
        /// Gets or sets the shipping address for the order.
        /// </summary>
        public required Address ShippingAddress { get; set; }

        /// <summary>
        /// Initializes a new instance of the Order class.
        /// </summary>
        public Order()
        {
            OrderId = Guid.NewGuid();
            OrderItems = new List<OrderItem>();
            Status = OrderStatus.InProgress;
        }

        /// <summary>
        /// Calculates the total amount of the order.
        /// </summary>
        public decimal TotalAmount => OrderItems.Sum(item => item.TotalPrice);

        /// <summary>
        /// Adds an order item to the order.
        /// </summary>
        /// <param name="item">The order item to add.</param>
        public void AddOrderItem(OrderItem item)
        {
            OrderItems.Add(item);
        }

        /// <summary>
        /// Removes an order item from the order.
        /// </summary>
        /// <param name="item">The order item to remove.</param>
        public void RemoveOrderItem(OrderItem item)
        {
            OrderItems.Remove(item);
        }

        /// <summary>
        /// Updates the status of the order.
        /// </summary>
        /// <param name="status">The new status of the order.</param>
        public void UpdateStatus(OrderStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Checks if the order is empty.
        /// </summary>
        /// <returns>True if the order has no items; otherwise, false.</returns>
        public bool IsEmpty() => !OrderItems.Any();

        /// <summary>
        /// Gets the total number of items in the order.
        /// </summary>
        public int ItemCount => OrderItems.Sum(item => item.Quantity);

        /// <summary>
        /// Gets the number of unique products in the order.
        /// </summary>
        public int UniqueItemCount => OrderItems.Count;

        /// <summary>
        /// Validates the total amount of the order against an expected total.
        /// </summary>
        /// <param name="expectedTotal">The expected total amount.</param>
        /// <returns>True if the total amount matches the expected total; otherwise, false.</returns>
        public bool ValidateTotalAmount(decimal expectedTotal)
        {
            return TotalAmount == expectedTotal;
        }
    }
}