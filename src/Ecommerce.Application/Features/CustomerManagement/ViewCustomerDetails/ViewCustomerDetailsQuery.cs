using MediatR;
using FluentResults;

namespace Ecommerce.Application.Features.CustomerManagement.ViewCustomerDetails
{
    public class ViewCustomerDetailsQuery : IRequest<Result<ViewCustomerDetailsResponse>>
    {
        public Guid CustomerId { get; set; }
    }
}