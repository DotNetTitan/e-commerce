using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.CategoryManagement.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<Result<DeleteCategoryResponse>>
    {
        public required Guid Id { get; init; }
    }
}