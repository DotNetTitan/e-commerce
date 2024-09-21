using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.OrderManagement.ListUserOrders
{
    public class ListUserOrdersQuery : IRequest<Result<ListUserOrdersResponse>>
    {
        public required Guid CustomerId { get; init; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}