using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Api.DTOs.Orders
{
    public class ListCustomerOrdersDto
    {
        public Guid CustomerId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public OrderStatus? StatusFilter { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public enum OrderStatus
    {
        // Add appropriate order status values
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}