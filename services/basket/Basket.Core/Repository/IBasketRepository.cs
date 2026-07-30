using Basket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Core.Repository
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasket(string userName);
        Task<ShoppingCart?> UpdateBasket(ShoppingCart shoppingCart);
        Task<bool> DeleteBasket(string userName);
    }
}
