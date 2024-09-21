using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.OrderManagement.GetOrderDetails
{
    public class GetOrderDetailsHandler : IRequestHandler<GetOrderDetailsQuery, Result<GetOrderDetailsResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderDetailsHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<GetOrderDetailsResponse>> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByOrderIdAsync(request.OrderId);

            if (order == null)
            {
                return Result.Fail<GetOrderDetailsResponse>($"Order with ID {request.OrderId} not found.");
            }

            if (order.CustomerId != request.CustomerId)
            {
                return Result.Fail<GetOrderDetailsResponse>("You are not authorized to view this order.");
            }

            var response = new GetOrderDetailsResponse
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(item => new OrderItemDetails
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? "Unknown Product",
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                }).ToList()
            };

            return Result.Ok(response);
        }
    }
}