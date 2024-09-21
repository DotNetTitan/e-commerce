﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.ReviewManagement.AddReview
{
    public class AddReviewResponse
    {
        public required Guid ReviewId { get; init; }
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
        public required DateTime ReviewDate { get; init; }
    }
}