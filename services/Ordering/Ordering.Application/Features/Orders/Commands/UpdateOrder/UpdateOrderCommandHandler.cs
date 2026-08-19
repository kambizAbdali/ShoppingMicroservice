using MapsterMapper;
using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Features.Orders.DTOs;
using Ordering.Core.Common;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    /// <summary>
    /// درخواست ویرایش اطلاعات یک سفارش
    /// </summary>
    public record UpdateOrderCommand : IRequest<OrderResponse>
    {
        /// <summary>
        /// شناسه سفارش موردنظر برای ویرایش
        /// </summary>
        public long Id { get; init; }

        /// <summary>
        /// نام کاربری خریدار
        /// </summary>
        public string? UserName { get; init; }

        /// <summary>
        /// مبلغ نهایی سفارش
        /// </summary>
        public decimal TotalPrice { get; init; }

        /// <summary>
        /// نام مشتری
        /// </summary>
        public string? FirstName { get; init; }

        /// <summary>
        /// نام خانوادگی مشتری
        /// </summary>
        public string? LastName { get; init; }

        /// <summary>
        /// ایمیل مشتری
        /// </summary>
        public string? EmailAddress { get; init; }

        /// <summary>
        /// آدرس ارسال سفارش
        /// </summary>
        public string? AddressLine { get; init; }

        /// <summary>
        /// استان یا ایالت
        /// </summary>
        public string? State { get; init; }

        /// <summary>
        /// روش پرداخت سفارش
        /// </summary>
        public PaymentMethodEnum PaymentMethod { get; init; }
    }

    /// <summary>
    /// Handler مربوط به ویرایش سفارش
    /// </summary>
    public sealed class UpdateOrderCommandHandler(
        IOrderRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<UpdateOrderCommand, OrderResponse>
    {
        public async Task<OrderResponse> Handle(
            UpdateOrderCommand request,
            CancellationToken cancellationToken)
        {
            var order = await repository.GetByIdAsync(request.Id);

            if (order is null)
                throw new NotFoundException(nameof(Order), request.Id.ToString());

            mapper.Map(request, order);

            await repository.UpdateAsync(order);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<OrderResponse>(order);
        }
    }
}