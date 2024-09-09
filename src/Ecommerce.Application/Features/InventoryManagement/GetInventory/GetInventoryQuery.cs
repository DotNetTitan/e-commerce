using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.InventoryManagement.GetInventory
{
    public class GetInventoryQuery : IRequest<Result<GetInventoryResponse>>
    {
        // Add any filtering or pagination parameters here if needed
    }
}