namespace Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents a product in the e-commerce system.
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public required decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the category identifier of the product.
        /// </summary>
        public required Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category of the product.
        /// </summary>
        public Category? Category { get; set; }

        /// <summary>
        /// Gets or sets the collection of reviews for the product.
        /// </summary>
        public ICollection<Review> Reviews { get; set; }

        /// <summary>
        /// Gets or sets the current stock quantity of the product.
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the low stock threshold for the product.
        /// </summary>
        public int LowStockThreshold { get; set; }

        /// <summary>
        /// Gets or sets the Stock Keeping Unit (SKU) of the product.
        /// </summary>
        public string SKU { get; private set; }

        /// <summary>
        /// Gets or sets the URL of the product's thumbnail.
        /// </summary>
        public string? ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the URLs of the product's images.
        /// </summary>
        public List<string> ImageUrls { get; set; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of the Product class.
        /// </summary>
        public Product()
        {
            ProductId = Guid.NewGuid();
            Reviews = new List<Review>();
            SKU = GenerateSKU();
        }

        /// <summary>
        /// Generates a unique SKU for the product.
        /// </summary>
        /// <returns>A string representing the unique SKU.</returns>
        private string GenerateSKU()
        {
            string categoryPrefix = CategoryId.ToString("N").Substring(0, 4).ToUpper();
            string productSuffix = ProductId.ToString("N").Substring(0, 8).ToUpper();
            return $"SKU-{categoryPrefix}-{productSuffix}";
        }

        /// <summary>
        /// Checks if the product is in stock for the requested quantity.
        /// </summary>
        /// <param name="quantity">The requested quantity.</param>
        /// <returns>True if the product is in stock for the requested quantity; otherwise, false.</returns>
        public bool IsInStock(int quantity) => StockQuantity >= quantity;

        /// <summary>
        /// Checks if the product is out of stock.
        /// </summary>
        public bool IsOutOfStock => StockQuantity <= 0;

        /// <summary>
        /// Decreases the stock quantity of the product.
        /// </summary>
        /// <param name="quantity">The quantity to decrease.</param>
        /// <exception cref="ArgumentException">Thrown when the quantity is negative.</exception>
        /// <exception cref="InvalidOperationException">Thrown when there's not enough stock.</exception>
        public void DecreaseStock(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantity));
            if (quantity > StockQuantity)
                throw new InvalidOperationException("Not enough stock");
            StockQuantity -= quantity;
        }

        /// <summary>
        /// Increases the stock quantity of the product.
        /// </summary>
        /// <param name="quantity">The quantity to increase.</param>
        /// <exception cref="ArgumentException">Thrown when the quantity is negative.</exception>
        public void IncreaseStock(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantity));
            StockQuantity += quantity;
        }

        /// <summary>
        /// Calculates the average rating of the product.
        /// </summary>
        public double AverageRating => Reviews.Any() ? Reviews.Average(r => r.Rating) : 0;

        /// <summary>
        /// Gets the number of reviews for the product.
        /// </summary>
        public int ReviewCount => Reviews.Count;

        /// <summary>
        /// Checks if the product is low on stock.
        /// </summary>
        /// <returns>True if the product is low on stock; otherwise, false.</returns>
        public bool IsLowStock()
        {
            return StockQuantity <= LowStockThreshold;
        }

        /// <summary>
        /// Updates the stock quantity of the product.
        /// </summary>
        /// <param name="newQuantity">The new stock quantity.</param>
        /// <exception cref="ArgumentException">Thrown when the new quantity is negative.</exception>
        public void UpdateStock(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative", nameof(newQuantity));
            
            int difference = newQuantity - StockQuantity;
            if (difference > 0)
                IncreaseStock(difference);
            else if (difference < 0)
                DecreaseStock(-difference);
        }

        /// <summary>
        /// Checks if the product can fulfill an order for the specified quantity.
        /// </summary>
        /// <param name="quantity">The requested quantity.</param>
        /// <returns>True if the product can fulfill the order; otherwise, false.</returns>
        public bool CanFulfillOrder(int quantity)
        {
            return IsInStock(quantity);
        }
    }
}