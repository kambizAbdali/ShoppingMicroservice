using Discount.Application.Protos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.gRPCService
{
    public class DiscountGRPCService(DiscountProtoService.DiscountProtoServiceClient client)
    {
        public async Task<CouponModel> GetDiscountByProductNameAsync(string productName)
        {
            var discountRequest = new GetDiscountByProductNameRequest()
            {
                ProductName = productName
            };
            return await client.GetDiscountByProductNameAsync(discountRequest);
        }

        public async Task<CouponModel> GetDiscountByProducIdAsync(string productId)
        {
            var discountRequest = new GetDiscountByProductIdRequest
            {
                ProductId = productId
            };
            return await client.GetDiscountByProductIdAsync(discountRequest);
        }

    }
}
