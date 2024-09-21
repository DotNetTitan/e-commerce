using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.OrderManagement.CancelOrder
{
    public class CancelOrderCommand : IRequest<Result<CancelOrderCommandResponse>>
    {
        public required Guid OrderId { get; init; }
        public required Guid CustomerId { get; init; }
    }
}
