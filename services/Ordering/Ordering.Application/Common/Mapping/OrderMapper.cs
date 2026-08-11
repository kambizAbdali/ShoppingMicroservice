using Mapster;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
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
    }
}
