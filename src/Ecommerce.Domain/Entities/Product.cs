namespace Ecommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            ProductId = Guid.NewGuid();
            Reviews = new List<Review>();
        }

        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public int StockQuantity { get; set; }
        public int LowStockThreshold { get; set; }

        // Check if the product is in stock for the requested quantity
        public bool IsInStock(int quantity) => StockQuantity >= quantity;

        // Check if the product is out of stock
        public bool IsOutOfStock => StockQuantity <= 0;

        // Decrease the stock quantity
        public void DecreaseStock(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantity));
            if (quantity > StockQuantity)
                throw new InvalidOperationException("Not enough stock");
            StockQuantity -= quantity;
        }

        // Increase the stock quantity
        public void IncreaseStock(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantity));
            StockQuantity += quantity;
        }

        // Calculate the average rating of the product
        public double AverageRating => Reviews.Any() ? Reviews.Average(r => r.Rating) : 0;

        // Get the number of reviews
        public int ReviewCount => Reviews.Count;

        // Check if the product is low on stock
        public bool IsLowStock()
        {
            return StockQuantity <= LowStockThreshold;
        }
    }
}