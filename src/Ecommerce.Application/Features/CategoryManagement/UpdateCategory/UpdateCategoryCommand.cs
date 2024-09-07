using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.CategoryManagement.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<Result<UpdateCategoryResponse>>
    {
        public required Guid CategoryId { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
    }
}