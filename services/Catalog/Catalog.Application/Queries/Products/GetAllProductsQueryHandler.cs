using AutoMapper;
using Catalog.Application.Responses;
using Catalog.Core.EntityParams;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Queries.Products
{
    public class GetAllProductsQuery : CatalogParams, IRequest<Pagination<ProductResponse>>
    {
    }

    public class GetAllProductsQueryHandler(IProductRepository repository, IMapper mapper) : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {
        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await repository.GetProductsAsync(request);
            return mapper.Map<Pagination<ProductResponse>>(products);
        }
    }
}
