using Basket.Core.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.CQRS.Commands
{
    public class DeleteBasketCommand : IRequest<bool>
    {
        public DeleteBasketCommand(string userName)
        {
            UserName = userName;
        }
        public string UserName { get; set; } = string.Empty;
    }
    public class DeleteBasketCommandHandler(IBasketRepository basketRepository) : IRequestHandler<DeleteBasketCommand, bool>
    {
        public async Task<bool> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            return await basketRepository.DeleteBasket(request.UserName);
        }
    }
}
