
using Basket.Api.Controllers;
using Basket.Application.CQRS.Commands;
using Basket.Application.CQRS.Queries.GetBasket;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    public class BasketController(IMediator mediator) : ApiController
    {
        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasketByUserName(string userName, CancellationToken CT)
        {
            var result = await mediator.Send(new GetBasketByUserNameQuery(userName), CT);
            return Ok(result);
        }

        //[HttpGet("{type}")]
        //public async Task<ActionResult<IEnumerable<TypeResponse>>> GetBasketsByTypeName(string type, CancellationToken CT)
        //{
        //    return Ok(await mediator.Send(new GetBasketsByTypeQuery(type), CT));
        //}

        [HttpPost]
        public async Task<ActionResult<ShoppingCartResponse>> CreateBasket(
            [FromBody] CreateBasketCommand requesr, CancellationToken CT)
        {
            var result = await mediator.Send(requesr, CT);
            return Ok(result);
        }

        //[HttpPut]
        //public async Task<ActionResult<bool>> UpdateBasket([FromBody] UpdateBasketCommand command, CancellationToken CT)
        //{
        //    return Ok(await mediator.Send(command, CT));
        //}

        [HttpDelete("{username}")]
        public async Task<ActionResult<bool>> DeleteBasket(string username, CancellationToken CT)
        {
            var result = await mediator.Send(new DeleteBasketCommand(username), CT);
            return Ok(result);
        }
    }
}