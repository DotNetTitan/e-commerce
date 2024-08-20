﻿namespace Ecommerce.Domain.Entities
{
    public class Payment
    {
        public Payment()
        {
            PaymentId = Guid.NewGuid();
        }

        public Guid PaymentId { get; set; }
        public required Guid OrderId { get; set; }
        public Order? Order { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime PaymentDate { get; set; }
        public required string PaymentMethod { get; set; }
    }
}