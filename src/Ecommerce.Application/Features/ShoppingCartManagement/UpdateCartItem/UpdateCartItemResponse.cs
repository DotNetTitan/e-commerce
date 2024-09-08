using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.ShoppingCartManagement.UpdateCartItem
{
    public class UpdateCartItemResponse
    {
        public required Guid CartId { get; init; }
        public required Guid ProductId { get; init; }
        public required int NewQuantity { get; init; }
        public required int TotalItems { get; init; }
        public required decimal TotalPrice { get; init; }
    }
}