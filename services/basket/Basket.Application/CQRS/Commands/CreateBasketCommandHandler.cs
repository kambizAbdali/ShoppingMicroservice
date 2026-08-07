using AutoMapper;
using Basket.Application.gRPCService;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repository;
using MediatR;

namespace Basket.Application.CQRS.Commands;

public sealed class CreateBasketCommand : IRequest<ShoppingCartResponse>
{
    public string UserName { get; set; } = string.Empty;
    public List<ShoppingCartItem> Items { get; set; } = [];
}

public sealed class CreateBasketCommandHandler(
    IBasketRepository repository,
    IMapper mapper,
    DiscountGRPCService discountGRPCService)
    : IRequestHandler<CreateBasketCommand, ShoppingCartResponse>
{
    public async Task<ShoppingCartResponse> Handle(
        CreateBasketCommand request,
        CancellationToken cancellationToken)
    {
        foreach (var item in request.Items)
        {
            if (string.IsNullOrWhiteSpace(item.ProductId))
                continue;

            var discount = await discountGRPCService.GetDiscountByProducIdAsync(item.ProductId);
            item.Price -= discount.Amount;
        }

        var shoppingCart = mapper.Map<ShoppingCart>(request);
        var updatedBasket = await repository.UpdateBasket(shoppingCart);

        return mapper.Map<ShoppingCartResponse>(updatedBasket);
    }
}
 