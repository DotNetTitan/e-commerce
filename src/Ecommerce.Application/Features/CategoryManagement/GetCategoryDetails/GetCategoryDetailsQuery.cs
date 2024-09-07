using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.CategoryManagement.GetCategoryDetails
{
    public class GetCategoryDetailsQuery : IRequest<Result<GetCategoryDetailsResponse>>
    {
        public required Guid Id { get; init; }
    }
}