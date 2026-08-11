using MediatR;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Exceptions;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteOrderCommandHandler(
        IOrderRepository repository,
        ILogger<DeleteOrderCommandHandler> logger)
        : IRequestHandler<DeleteOrderCommand, bool>
    {
        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate input
            if (request.Id <= 0)
            {
                logger.LogWarning("Invalid Id provided for DeleteOrderCommand: {OrderId}", request.Id);
                throw new ValidationException(
                    nameof(request.Id),
                    request.Id.ToString(),
                    "Order Id must be greater than zero.");
            }

            // 2. Get entity and check for existence
            var orderToDelete = await repository.GetByIdAsync(request.Id);
            if (orderToDelete == null)
            {
                logger.LogWarning("Order with Id {OrderId} not found. Cannot delete.", request.Id);
                throw new NotFoundException(nameof(Order), request.Id.ToString());
            }

            // 3. Attempt to delete
            var deletedOrder = await repository.DeleteAsync(orderToDelete);
            if (deletedOrder == null)
            {
                logger.LogError("Failed to delete order with Id {OrderId}. Repository returned null after delete operation.", request.Id);
                throw new OperationFailedException("Delete", nameof(Order), request.Id.ToString());
            }

            logger.LogInformation("Order with Id {OrderId} deleted successfully.", request.Id);
            return true;
        }
    }
}