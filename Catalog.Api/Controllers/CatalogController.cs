using Catalog.Application.Commands.Products;
using Catalog.Application.Queries.Brands;
using Catalog.Application.Queries.Products;
using Catalog.Application.Queries.Types;
using Catalog.Application.Responses;
using Catalog.Core.EntityParams;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    public class CatalogController(IMediator mediator) : ApiController
    {
        //IActionResult ==> No Output
        //ActionResult ==> Output => shows in swagger
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductById(string id, CancellationToken CT)
        {
            return Ok(await mediator.Send(new GetProductByIdQuery(id), CT));
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByName(string name, CancellationToken CT)
        {
            return Ok(await mediator.Send(new GetProductsByNameQuery(name), CT));
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductResponse>>> GetAllProducts(
            [FromQuery] GetAllProductsQuery request, CancellationToken CT)
        {
            // نوع بازگشتی باید با Pagination هماهنگ باشد، نه IEnumerable
            return Ok(await mediator.Send(request, CT));
        }


        [HttpGet("{brand}")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByBrandName(string brand, CancellationToken CT)
        {
            return Ok(await mediator.Send(new GetAllProductsByBrandQuery(brand), CT));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandResponse>>> GetAllProductBrands(CancellationToken CT)
        {

            return Ok(await mediator.Send(new GetAllProductBrandsQuery(), CT));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeResponse>>> GetAllProductTypes(CancellationToken CT)
        {
            return Ok(await mediator.Send(new GetAllProductTypesQuery(), CT));
        }

        [HttpGet("{type}")]
        public async Task<ActionResult<IEnumerable<TypeResponse>>> GetProductsByTypeName(string type, CancellationToken CT)
        {
            return Ok(await mediator.Send(new GetProductsByTypeQuery(type), CT));
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductCommand command, CancellationToken CT)
        {
            return Ok(await mediator.Send(command, CT));
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateProduct([FromBody] UpdateProductCommand command, CancellationToken CT)
        {
            return Ok(await mediator.Send(command, CT));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProduct(string id, CancellationToken CT)
        {
            return Ok(await mediator.Send(new DeleteProductCommand(id), CT));
        }
    }
}