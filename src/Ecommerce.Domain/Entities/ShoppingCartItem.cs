namespace Ecommerce.Domain.Entities
{
    public class ShoppingCartItem : BaseEntity
    {
        public ShoppingCartItem()
        {
            ShoppingCartItemId = Guid.NewGuid();
        }

        public Guid ShoppingCartItemId { get; set; }
        public required Guid ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        public required Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public required int Quantity { get; set; }
        public required decimal Price { get; set; }

        // Calculate the total price for this item
        public decimal TotalPrice => Quantity * Price;

        // Increase the quantity of this item
        public void IncreaseQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));
            Quantity += amount;
        }

        // Decrease the quantity of this item
        public void DecreaseQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));
            Quantity = Math.Max(0, Quantity - amount);
        }

        // Update the quantity of this item
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity must be non-negative", nameof(newQuantity));
            Quantity = newQuantity;
        }

        // Check if this item is available (assuming Product is fully loaded)
        public bool IsAvailable => Product?.StockQuantity > 0;

        // Check if the requested quantity is available
        public bool IsQuantityAvailable(int requestedQuantity)
        {
            return Product?.StockQuantity >= requestedQuantity;
        }
    }
}