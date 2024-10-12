using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents a shopping cart in the e-commerce system.
    /// </summary>
    public class ShoppingCart : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the shopping cart.
        /// </summary>
        public Guid ShoppingCartId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier associated with this shopping cart.
        /// </summary>
        public required Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer associated with this shopping cart.
        /// </summary>
        public Customer? Customer { get; set; }

        /// <summary>
        /// Gets or sets the collection of items in the shopping cart.
        /// </summary>
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        /// <summary>
        /// Initializes a new instance of the ShoppingCart class.
        /// </summary>
        public ShoppingCart()
        {
            ShoppingCartId = Guid.NewGuid();
            ShoppingCartItems = new List<ShoppingCartItem>();
        }

        /// <summary>
        /// Calculates the total number of items in the cart.
        /// </summary>
        public int TotalItems => ShoppingCartItems.Sum(item => item.Quantity);

        /// <summary>
        /// Calculates the total price of all items in the cart.
        /// </summary>
        public decimal TotalPrice => ShoppingCartItems.Sum(item => item.TotalPrice);

        /// <summary>
        /// Adds a new item to the cart or updates the quantity if it already exists.
        /// </summary>
        /// <param name="product">The product to add to the cart.</param>
        /// <param name="quantity">The quantity of the product to add.</param>
        public void AddItem(Product product, int quantity)
        {
            var existingItem = ShoppingCartItems.FirstOrDefault(item => item.ProductId == product.ProductId);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(quantity);
            }
            else
            {
                ShoppingCartItems.Add(new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    ProductId = product.ProductId,
                    Product = product,
                    Quantity = quantity,
                    Price = product.Price
                });
            }
        }

        /// <summary>
        /// Removes an item from the cart.
        /// </summary>
        /// <param name="productId">The identifier of the product to remove.</param>
        public void RemoveItem(Guid productId)
        {
            var item = ShoppingCartItems.FirstOrDefault(item => item.ProductId == productId);
            if (item != null)
            {
                ShoppingCartItems.Remove(item);
            }
        }

        /// <summary>
        /// Clears all items from the cart.
        /// </summary>
        public void Clear()
        {
            ShoppingCartItems.Clear();
        }

        /// <summary>
        /// Checks if the cart is empty.
        /// </summary>
        public bool IsEmpty => !ShoppingCartItems.Any();

        /// <summary>
        /// Gets the number of unique products in the cart.
        /// </summary>
        public int UniqueItemCount => ShoppingCartItems.Count;
    }
}