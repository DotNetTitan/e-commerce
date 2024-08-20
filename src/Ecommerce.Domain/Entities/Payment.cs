using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    public class Payment
    {
        public Payment()
        {
            PaymentId = Guid.NewGuid();
        }

        public Guid PaymentId { get; set; }
        public required Guid OrderId { get; set; }
        public required Order Order { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime PaymentDate { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }
    }
}