namespace Ecommerce.Domain.Entities
{
    public class OrderItem
    {
        public OrderItem()
        {
            OrderItemId = Guid.NewGuid();
        }

        public Guid OrderItemId { get; set; }
        public required Guid OrderId { get; set; }
        public required Order Order { get; set; }
        public required Guid ProductId { get; set; }
        public required Product Product { get; set; }
        public required int Quantity { get; set; }
        public required decimal Price { get; set; }
    }
}