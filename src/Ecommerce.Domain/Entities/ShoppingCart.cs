using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Domain.Entities
{
    public class ShoppingCart : BaseEntity
    {
        public ShoppingCart()
        {
            ShoppingCartId = Guid.NewGuid();
            ShoppingCartItems = new List<ShoppingCartItem>();
        }

        public Guid ShoppingCartId { get; set; }
        public required Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        // Calculate the total number of items in the cart
        public int TotalItems => ShoppingCartItems.Sum(item => item.Quantity);

        // Calculate the total price of all items in the cart
        public decimal TotalPrice => ShoppingCartItems.Sum(item => item.TotalPrice);

        // Add a new item to the cart or update quantity if it already exists
        public void AddItem(Product product, int quantity)
        {
            var existingItem = ShoppingCartItems.FirstOrDefault(item => item.ProductId == product.ProductId);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
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

        // Remove an item from the cart
        public void RemoveItem(Guid productId)
        {
            var item = ShoppingCartItems.FirstOrDefault(item => item.ProductId == productId);
            if (item != null)
            {
                ShoppingCartItems.Remove(item);
            }
        }

        // Clear all items from the cart
        public void Clear()
        {
            ShoppingCartItems.Clear();
        }

        // Check if the cart is empty
        public bool IsEmpty => !ShoppingCartItems.Any();

        // Get the number of unique products in the cart
        public int UniqueItemCount => ShoppingCartItems.Count;
    }
}