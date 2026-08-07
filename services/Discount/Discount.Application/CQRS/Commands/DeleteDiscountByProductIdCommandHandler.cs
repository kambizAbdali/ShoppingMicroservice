using Discount.Core.Interfaces;
using MediatR;

namespace Discount.Application.CQRS.Commands;

public sealed record DeleteDiscountByProductIdCommand(string ProductId) : IRequest<bool>;

public sealed class DeleteDiscountByProductIdCommandHandler
    : IRequestHandler<DeleteDiscountByProductIdCommand, bool>
{
    private readonly IDiscountRepository _repository;

    public DeleteDiscountByProductIdCommandHandler(IDiscountRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<bool> Handle(
        DeleteDiscountByProductIdCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.ProductId);

        var deleted = await _repository.DeleteDiscountByProductIdAsync(request.ProductId);

        if (!deleted)
        {
            throw new InvalidOperationException(
                $"Deleting coupon failed for ProductId: {request.ProductId}");
        }

        return true;
    }
}
