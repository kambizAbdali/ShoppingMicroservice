using AutoMapper;
using Discount.Application.Protos;
using Discount.Core.Interfaces;
using Grpc.Core;
using MediatR;
using static Grpc.Core.Metadata;

namespace Discount.Application.CQRS.Queries;

public sealed record GetDiscountByProductNameQuery(string ProductName) : IRequest<CouponModel>;

public sealed class GetDiscountByProductNameQueryHandler
    : IRequestHandler<GetDiscountByProductNameQuery, CouponModel>
{
    private readonly IDiscountRepository _repository;
    private readonly IMapper _mapper;

    public GetDiscountByProductNameQueryHandler(IDiscountRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CouponModel> Handle(
        GetDiscountByProductNameQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.ProductName);

        var entity = await _repository.GetDiscountByProductNameAsync(request.ProductName);

        // If not found, throw a gRPC exception with NotFound status
        if (entity is null)
        {
            throw new RpcException(
                new Status(StatusCode.NotFound, $"Discount not found for product: '{request.ProductName}'")
            );
        }

        return _mapper.Map<CouponModel>(entity);
    }
}
