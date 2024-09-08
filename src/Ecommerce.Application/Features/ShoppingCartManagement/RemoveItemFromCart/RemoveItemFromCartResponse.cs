using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.ShoppingCartManagement.RemoveItemFromCart
{
    public class RemoveItemFromCartResponse
    {
        public required Guid CartId { get; init; }
        public required Guid RemovedProductId { get; init; }
        public required int TotalItems { get; init; }
        public required decimal TotalPrice { get; init; }
    }
}