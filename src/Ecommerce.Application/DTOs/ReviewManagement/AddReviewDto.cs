﻿using System;

namespace Ecommerce.Application.DTOs.ReviewManagement
{
    public class AddReviewDto
    {
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
    }
}