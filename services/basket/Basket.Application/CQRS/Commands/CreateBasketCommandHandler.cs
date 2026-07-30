using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.CQRS.Commands
{
    public class CreateBasketCommand : IRequest<ShoppingCartResponse>
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; }
    }
    public class CreateBasketCommandHandler(IBasketRepository repository, IMapper mapper) : IRequestHandler<CreateBasketCommand, ShoppingCartResponse>
    {
        public async Task<ShoppingCartResponse> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {

            var shoppingCart = mapper.Map<ShoppingCart>(request);
            var createdBasket = await repository.UpdateBasket(shoppingCart);
            return mapper.Map<ShoppingCartResponse>(createdBasket);
        }
    }
}
