using AutoMapper;
using Discount.Application.Protos;
using Discount.Core.Entities;
using Discount.Core.Interfaces;
using Grpc.Core;
using MediatR;

namespace Discount.Application.CQRS.Commands
{
    /// <summary>
    /// Command to create a new discount.
    /// </summary>
    public sealed record CreateDiscountCommand : IRequest<CouponModel>
    {
        /// <summary>
        /// The coupon data to be created.
        /// </summary>
        public CouponModel CouponModel { get; init; }

        public CreateDiscountCommand(CouponModel couponModel)
        {
            CouponModel = couponModel;
        }
    }

    /// <summary>
    /// Handler for <see cref="CreateDiscountCommand"/>.
    /// </summary>
    public sealed class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;

        public CreateDiscountCommandHandler(IDiscountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            // Map the gRPC model to the domain entity
            var couponEntity = _mapper.Map<Coupon>(request.CouponModel);

            // Persist the entity
            var created = await _repository.CreateDiscountAsync(couponEntity);

            if (!created)
            {
                // Throw a gRPC exception with appropriate status
                throw new RpcException(
                    new Status(StatusCode.Internal, "Failed to create discount.")
                );
            }

            // Assuming the repository updates the entity with the generated Id,
            // map it back to the response model
            var resultModel = _mapper.Map<CouponModel>(couponEntity);

            return resultModel;
        }

    }
}