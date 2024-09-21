using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ReviewManagement.AddReview
{
    public class AddReviewCommand : IRequest<Result<AddReviewResponse>>
    {
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
    }
}