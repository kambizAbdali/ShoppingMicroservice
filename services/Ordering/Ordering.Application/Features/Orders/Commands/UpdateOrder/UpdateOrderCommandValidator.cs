using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    /// <summary>
    /// Validator مربوط به درخواست ویرایش سفارش
    /// </summary>
    public sealed class UpdateOrderCommandValidator
        : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Order Id must be greater than zero.");

            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("UserName is required.")
                .MaximumLength(100)
                .WithMessage("UserName cannot exceed 100 characters.");

            RuleFor(x => x.TotalPrice)
                .GreaterThan(0)
                .WithMessage("TotalPrice must be greater than zero.");

            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("FirstName is required.")
                .MaximumLength(100)
                .WithMessage("FirstName cannot exceed 100 characters.");

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("LastName is required.")
                .MaximumLength(100)
                .WithMessage("LastName cannot exceed 100 characters.");

            RuleFor(x => x.EmailAddress)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("EmailAddress is required.")
                .EmailAddress()
                .WithMessage("EmailAddress is not valid.")
                .MaximumLength(200)
                .WithMessage("EmailAddress cannot exceed 200 characters.");

            RuleFor(x => x.AddressLine)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("AddressLine is required.")
                .MaximumLength(500)
                .WithMessage("AddressLine cannot exceed 500 characters.");

            RuleFor(x => x.State)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("State is required.")
                .MaximumLength(100)
                .WithMessage("State cannot exceed 100 characters.");

            RuleFor(x => x.PaymentMethod)
                .IsInEnum()
                .WithMessage("PaymentMethod is not valid.");
        }
    }
}
