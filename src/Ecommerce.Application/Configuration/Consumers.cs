﻿using Ecommerce.Application.Features.Orders.EventHandlers.OrderPlaced;
using Ecommerce.Application.Features.Orders.EventHandlers.OrderCancelled;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Application.Configuration
{
    public static class Consumers
    {
        public static void AddConsumers(this IServiceCollection services)
        {
            services.AddScoped<OrderPlacedEventHandler>();
            services.AddScoped<OrderCancelledEventHandler>();
        }
    }
}