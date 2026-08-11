using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Features.Orders.DTOs;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersByUserName
{
    public record GetOrdersByUserNameQuery(string Username) : IRequest<IEnumerable<OrderResponse>>;

    public class GetOrdersByUserNameQueryHandler(
        IOrderRepository repository,
        IMapper mapper,
        ILogger<GetOrdersByUserNameQueryHandler> logger)
        : IRequestHandler<GetOrdersByUserNameQuery, IEnumerable<OrderResponse>>
    {
        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersByUserNameQuery request, CancellationToken cancellationToken)
        {
            // 1. Validate input
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                logger.LogWarning("Username is null or empty for GetOrdersByUserNameQuery");
                throw new ValidationException(
                    nameof(request.Username),
                    request.Username,
                    "Username is required.");
            }

            try
            {
                // 2. Get orders from repository
                logger.LogInformation("Retrieving orders for Username: {Username}", request.Username);
                var orders = await repository.GetOrdersByUserNameAsync(request.Username);

                // 3. Check if orders exist
                if (orders == null || !orders.Any())
                {
                    logger.LogWarning("No orders found for Username: {Username}", request.Username);
                    throw new NotFoundException(
                        nameof(Order),
                        $"Username '{request.Username}' has no orders.");
                }

                // 4. Map to response DTOs
                var response = mapper.Map<IEnumerable<OrderResponse>>(orders);

                logger.LogInformation("Successfully retrieved {OrderCount} orders for Username: {Username}",
                    response.Count(), request.Username);

                return response;
            }
            catch (NotFoundException)
            {
                // Re-throw NotFoundException as is
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving orders for Username: {Username}", request.Username);
                throw new OperationFailedException(
                    "GetOrdersByUserName",
                    nameof(Order),
                    $"Username: {request.Username}",
                    ex);
            }
        }
    }
}