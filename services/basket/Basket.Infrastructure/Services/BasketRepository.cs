using Basket.Core.Entities;
using Basket.Core.Repository;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Infrastructure.Services
{
    public class BasketRepository(IDistributedCache redis) : IBasketRepository
    {
        private static string GetBasketKey(string userName) => $"basket:{userName}";
        public async Task<bool> DeleteBasket(string userName)
        {
            await redis.RemoveAsync(GetBasketKey(userName));
            return true;
        }

        public async Task<ShoppingCart?> GetBasket(string userName)
        {
            var basket = await redis.GetStringAsync(GetBasketKey(userName));
            return string.IsNullOrEmpty(basket) ? null : JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart?> UpdateBasket(ShoppingCart shoppingCart)
        {
            if (string.IsNullOrEmpty(shoppingCart.UserName))
            {
                throw new ArgumentException("UserName cannot be null or empty", nameof(shoppingCart.UserName));
            }
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
            };

            var json = JsonConvert.SerializeObject(shoppingCart);

            await redis.SetStringAsync(GetBasketKey(shoppingCart.UserName), json, options);
            return await GetBasket(shoppingCart.UserName);
        }
    }
}
