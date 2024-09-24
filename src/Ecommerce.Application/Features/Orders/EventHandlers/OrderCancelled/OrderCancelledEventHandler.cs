using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Application.Features.Orders.EventHandlers.OrderCancelled
{
    public class OrderCancelledEventHandler : IConsumer<OrderCancelledEvent>
    {
        private readonly ILogger<OrderCancelledEventHandler> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailService _emailService;

        public OrderCancelledEventHandler(ILogger<OrderCancelledEventHandler> logger, IProductRepository productRepository,
            IEmailService emailService, IOrderRepository orderRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<OrderCancelledEvent> context)
        {
            var notification = context.Message;
            _logger.LogInformation($"Order placed: {notification.OrderId}, Customer: {notification.CustomerId}");

            // Update stock
            var order = await _orderRepository.GetOrderByOrderIdAsync(notification.OrderId);
            if (order != null)
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.IncreaseStock(item.Quantity);
                        await _productRepository.UpdateAsync(product);
                    }
                }
            }

            // Send email notification
            await _emailService.SendOrderCancellationEmail(notification.OrderId, notification.Email);
        }
    }
}