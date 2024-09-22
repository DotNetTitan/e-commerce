using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.CategoryManagement.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Result<CreateCategoryCommandResponse>>
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
    }
}