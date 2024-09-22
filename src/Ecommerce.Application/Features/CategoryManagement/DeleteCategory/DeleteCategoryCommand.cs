using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.CategoryManagement.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<Result<DeleteCategoryCommandResponse>>
    {
        public required Guid CategoryId { get; init; }
    }
}