using Discount.Application.CQRS.Commands;
using Discount.Application.CQRS.Queries;
using Discount.Application.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Api.Services;

public class DiscountService(IMediator mediator) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscountByProductId(GetDiscountByProductIdRequest request, ServerCallContext context)
    {
        return await mediator.Send(new GetDiscountByProductIdQuery(request.ProductId));
    }

    public override async Task<CouponModel> GetDiscountByProductName(GetDiscountByProductNameRequest request, ServerCallContext context)
    {
        return await mediator.Send(new GetDiscountByProductNameQuery(request.ProductName));
    }

    public async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        return await mediator.Send(new CreateDiscountCommand(request.Coupon));
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        return await mediator.Send(new UpdateDiscountCommand(request.Coupon));
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var isDeleted = await mediator.Send(new DeleteDiscountByProductIdCommand(request.ProductId));
        return new DeleteDiscountResponse { Success = isDeleted };
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscountByName(DeleteDiscountByNameRequest request, ServerCallContext context)
    {
        var isDeleted = await mediator.Send(new DeleteDiscountByProductNameCommand(request.ProductName));
        return new DeleteDiscountResponse { Success = isDeleted };
    }
}
