using System;

namespace Ecommerce.Application.DTOs.OrderManagement
{
    public class CancelOrderDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
