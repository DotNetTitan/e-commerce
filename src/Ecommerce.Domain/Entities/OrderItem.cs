namespace Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents an item within an order in the e-commerce system.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order item.
        /// </summary>
        public Guid OrderItemId { get; private set; }

        /// <summary>
        /// Gets or sets the order identifier associated with this item.
        /// </summary>
        public required Guid OrderId { get; init; }

        /// <summary>
        /// Gets or sets the order associated with this item.
        /// </summary>
        public Order? Order { get; init; }

        /// <summary>
        /// Gets or sets the product identifier associated with this item.
        /// </summary>
        public required Guid ProductId { get; init; }

        /// <summary>
        /// Gets or sets the product associated with this item.
        /// </summary>
        public Product? Product { get; init; }

        /// <summary>
        /// Gets or sets the quantity of the product in this order item.
        /// </summary>
        public required int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product in this order item.
        /// </summary>
        public required decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the total price for this order item.
        /// </summary>
        public decimal TotalPrice { get; private set; }

        /// <summary>
        /// Initializes a new instance of the OrderItem class.
        /// </summary>
        public OrderItem()
        {
            OrderItemId = Guid.NewGuid();
        }

        /// <summary>
        /// Calculates and updates the total price for this order item.
        /// </summary>
        public void CalculateTotalPrice()
        {
            TotalPrice = Quantity * UnitPrice;
        }
    }
}