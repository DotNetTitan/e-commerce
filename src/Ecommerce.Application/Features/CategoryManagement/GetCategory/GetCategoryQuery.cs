using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.CategoryManagement.GetCategory
{
    public class GetCategoryQuery : IRequest<Result<GetCategoryQueryResponse>>
    {
        public required Guid CategoryId { get; init; }
    }
}