using MapsterMapper;
using MediatR;
using Ordering.Application.Features.Orders.DTOs;
using Ordering.Core.Common;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Application.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public record CheckoutOrderCommand : IRequest<OrderResponse>
    {
        public string? UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? AddressLine { get; set; }
        public string? State { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
    }

    public class CheckoutOrderCommandHandler(
        IOrderRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CheckoutOrderCommandHandler> logger)
        : IRequestHandler<CheckoutOrderCommand, OrderResponse>
    {
        public async Task<OrderResponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate required fields
            if (string.IsNullOrWhiteSpace(request.EmailAddress))
            {
                logger.LogWarning("EmailAddress is required for checkout order");
                throw new ValidationException(
                    nameof(request.EmailAddress),
                    request.EmailAddress,
                    "EmailAddress is required.");
            }

            if (request.TotalPrice <= 0)
            {
                logger.LogWarning("TotalPrice must be greater than zero. Value: {TotalPrice}", request.TotalPrice);
                throw new ValidationException(
                    nameof(request.TotalPrice),
                    request.TotalPrice.ToString(),
                    "TotalPrice must be greater than zero.");
            }

            try
            {
                // 2. Convert Command to Entity
                var orderEntity = mapper.Map<Order>(request);

                // 3. Add to Repository (only tracked in EF Core memory)
                await repository.AddAsync(orderEntity);

                // 4. Finalize transaction in database (call SaveChangesAsync via UnitOfWork)
                await unitOfWork.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Order created successfully with Id {OrderId} for User {UserName}",
                    orderEntity.Id, request.UserName);

                // 5. Convert saved entity (which now has generated Id) to DTO
                return mapper.Map<OrderResponse>(orderEntity);
            }
            catch (DbUpdateException dbEx)
            {
                logger.LogError(dbEx, "Database error occurred while creating order for User {UserName}", request.UserName);
                throw new OperationFailedException(
                    "Create",
                    nameof(Order),
                    "Checkout",
                    dbEx);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occurred while creating order for User {UserName}", request.UserName);
                throw new OperationFailedException(
                    "Create",
                    nameof(Order),
                    "Checkout",
                    ex);
            }
        }
    }
}