using FluentValidation;
using MediatR;
using System.Linq; // حتماً این را اضافه کنید

namespace Ordering.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // اگر هیچ Validator ای ثبت نشده، برو سراغ مرحله بعد
            if (!validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                // ساخت پیام خطا بر اساس لیست شکست‌ها
                var errorMessage = string.Join(" | ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));

                // پرتاب اکسپشن کاستوم
                throw new Ordering.Application.Common.Exceptions.ValidationException (errorMessage);
            }

            return await next();
        }
    }
}
