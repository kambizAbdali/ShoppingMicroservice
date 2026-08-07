using AutoMapper;
using Discount.Application.Protos;
using Discount.Core.Entities;
using Discount.Core.Interfaces;
using Grpc.Core;
using MediatR;

namespace Discount.Application.CQRS.Queries
{
    /// <summary>
    /// Query to retrieve a discount by product Id.
    /// </summary>
    public sealed record GetDiscountByProductIdQuery : IRequest<CouponModel>
    {
        /// <summary>
        /// Id of the product to search for.
        /// </summary>
        public string ProductId { get; init; }

        public GetDiscountByProductIdQuery(string productId)
        {
            ProductId = productId;
        }
    }

    /// <summary>
    /// Handler for <see cref="GetDiscountByProductIdQuery"/>.
    /// </summary>
    public sealed class GetDiscountByProductIdQueryHandler : IRequestHandler<GetDiscountByProductIdQuery, CouponModel>
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;

        public GetDiscountByProductIdQueryHandler(IDiscountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CouponModel> Handle(GetDiscountByProductIdQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the discount entity from the repository
            var entity = await _repository.GetDiscountByProductIdAsync(request.ProductId);

            // If not found, throw a gRPC exception with NotFound status
            if (entity is null)
            {
                throw new RpcException(
                    new Status(StatusCode.NotFound, $"Discount not found for product: '{request.ProductId}'")
                );
            }

            // Map the domain entity to the gRPC response model
            return _mapper.Map<CouponModel>(entity);
        }
    }
}