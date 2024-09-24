using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Application.Features.Orders.EventHandlers.OrderPlaced
{
    public class OrderPlacedEventHandler : IConsumer<OrderPlacedEvent>
    {
        private readonly ILogger<OrderPlacedEventHandler> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailService _emailService;

        public OrderPlacedEventHandler(ILogger<OrderPlacedEventHandler> logger, IProductRepository productRepository,
            IEmailService emailService, IOrderRepository orderRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
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
                        product.DecreaseStock(item.Quantity);
                        await _productRepository.UpdateAsync(product);
                    }
                }
            }

            // Send email notification
            await _emailService.SendOrderConfirmationEmail(notification.OrderId, notification.Email);
        }
    }
}