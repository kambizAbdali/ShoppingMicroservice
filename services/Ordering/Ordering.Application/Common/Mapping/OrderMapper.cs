using Mapster;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.DTOs;
using Ordering.Core.Entities;

public class OrderMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // TwoWays means RevereseMap (in automapper)
        config.NewConfig<Order, OrderResponse>()
              .TwoWays();

        config.NewConfig<CheckoutOrderCommand, Order>()
            .TwoWays();

        config.NewConfig<UpdateOrderCommand, Order>()
            // فیلدهای سیستمی نباید توسط کاربر ویرایش شوند.
            .Ignore(destination => destination.Id)
            .Ignore(destination => destination.CreatedBy)
            .Ignore(destination => destination.CreatedDate)
            .Ignore(destination => destination.LastModifiedBy)
            .Ignore(destination => destination.LastModifiedDate)
            .Ignore(destination => destination.Version)
            .Ignore(destination => destination.IsActive)
            .TwoWays();

        config.NewConfig<Order, OrderResponse>();
    }
}
