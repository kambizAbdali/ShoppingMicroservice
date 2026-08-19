using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public sealed class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Order Id is required.")
                .GreaterThan(0)
                .WithMessage("Order Id must be greater than zero.");
        }
    }
}
