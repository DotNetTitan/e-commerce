﻿namespace Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents an item within a shopping cart in the e-commerce system.
    /// </summary>
    public class ShoppingCartItem : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the shopping cart item.
        /// </summary>
        public Guid ShoppingCartItemId { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart identifier associated with this item.
        /// </summary>
        public required Guid ShoppingCartId { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart associated with this item.
        /// </summary>
        public ShoppingCart? ShoppingCart { get; set; }

        /// <summary>
        /// Gets or sets the product identifier associated with this item.
        /// </summary>
        public required Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product associated with this item.
        /// </summary>
        public Product? Product { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product in this shopping cart item.
        /// </summary>
        public required int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price of the product in this shopping cart item.
        /// </summary>
        public required decimal Price { get; set; }

        /// <summary>
        /// Initializes a new instance of the ShoppingCartItem class.
        /// </summary>
        public ShoppingCartItem()
        {
            ShoppingCartItemId = Guid.NewGuid();
        }

        /// <summary>
        /// Calculates the total price for this shopping cart item.
        /// </summary>
        public decimal TotalPrice => Quantity * Price;

        /// <summary>
        /// Increases the quantity of this item.
        /// </summary>
        /// <param name="amount">The amount to increase the quantity by.</param>
        /// <exception cref="ArgumentException">Thrown when the amount is negative.</exception>
        public void IncreaseQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));
            Quantity += amount;
        }

        /// <summary>
        /// Decreases the quantity of this item.
        /// </summary>
        /// <param name="amount">The amount to decrease the quantity by.</param>
        /// <exception cref="ArgumentException">Thrown when the amount is negative.</exception>
        public void DecreaseQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));
            Quantity = Math.Max(0, Quantity - amount);
        }

        /// <summary>
        /// Updates the quantity of this item.
        /// </summary>
        /// <param name="newQuantity">The new quantity to set.</param>
        /// <exception cref="ArgumentException">Thrown when the new quantity is negative.</exception>
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity must be non-negative", nameof(newQuantity));
            Quantity = newQuantity;
        }

        /// <summary>
        /// Checks if this item is available (assuming Product is fully loaded).
        /// </summary>
        public bool IsAvailable => Product?.StockQuantity > 0;

        /// <summary>
        /// Checks if the requested quantity is available.
        /// </summary>
        /// <param name="requestedQuantity">The quantity to check for availability.</param>
        /// <returns>True if the requested quantity is available; otherwise, false.</returns>
        public bool IsQuantityAvailable(int requestedQuantity)
        {
            return Product?.StockQuantity >= requestedQuantity;
        }
    }
}