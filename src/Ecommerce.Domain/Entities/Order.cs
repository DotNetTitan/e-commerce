using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderId = Guid.NewGuid();
            OrderItems = new List<OrderItem>();
        }

        public Guid OrderId { get; set; }
        public required DateTime OrderDate { get; set; }
        public required Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public required Address ShippingAddress { get; set; }
    }
}