using AutoMapper;
using Discount.Application.Protos;
using Discount.Core.Entities;
using Discount.Core.Interfaces;
using MediatR;

namespace Discount.Application.CQRS.Commands;

public sealed record UpdateDiscountCommand(CouponModel Model) : IRequest<CouponModel>;

public sealed class UpdateDiscountCommandHandler
    : IRequestHandler<UpdateDiscountCommand, CouponModel>
{
    private readonly IDiscountRepository _repository;
    private readonly IMapper _mapper;

    public UpdateDiscountCommandHandler(IDiscountRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Model);

        var couponEntity = _mapper.Map<Coupon>(request.Model);

        var updated = await _repository.UpdateDiscountAsync(couponEntity);

        if (!updated)
            throw new InvalidOperationException($"Updating coupon failed. Coupon Id: {couponEntity.Id}");

        return request.Model;
    }
}
