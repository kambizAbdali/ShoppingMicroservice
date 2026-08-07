using Discount.Core.Interfaces;
using MediatR;

namespace Discount.Application.CQRS.Commands;

public sealed record DeleteDiscountByProductNameCommand(string ProductName)
    : IRequest<bool>;

public sealed class DeleteDiscountByProductNameCommandHandler
    : IRequestHandler<DeleteDiscountByProductNameCommand, bool>
{
    private readonly IDiscountRepository _repository;

    public DeleteDiscountByProductNameCommandHandler(
        IDiscountRepository repository)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<bool> Handle(
        DeleteDiscountByProductNameCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.ProductName);

        return await _repository.DeleteDiscountByProductNameAsync(
            request.ProductName);
    }
}