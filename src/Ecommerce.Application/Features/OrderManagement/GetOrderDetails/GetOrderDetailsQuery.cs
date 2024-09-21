using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.OrderManagement.GetOrderDetails
{
    public class GetOrderDetailsQuery : IRequest<Result<GetOrderDetailsResponse>>
    {
        public required Guid OrderId { get; init; }
        public required Guid CustomerId { get; init; }
    }
}