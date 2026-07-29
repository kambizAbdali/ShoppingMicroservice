using AutoMapper;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Queries.Products
{
    public class GetAllProductsByBrandQuery : IRequest<IEnumerable<ProductResponse>>
    {
        public GetAllProductsByBrandQuery(string brand)
        {
            Brand = brand;
        }
        public string Brand { get; set; }
    }

    public class GetAllProductsByBrandQueryHandler(IProductRepository productRepository, IMapper mapper) :
        IRequestHandler<GetAllProductsByBrandQuery, IEnumerable<ProductResponse>>
    {
        public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            var result = await productRepository.GetProductByBrandAsync(request.Brand);
            return mapper.Map<IEnumerable<ProductResponse>>(result);
        }
    }
}