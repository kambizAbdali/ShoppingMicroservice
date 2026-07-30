using Basket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Responses
{
    public class ShoppingCartResponse
    {
        public string? UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = [];

        public ShoppingCartResponse(string userName)
        {
            UserName = userName;
        }
        public decimal CalculateOroginalPrice()
        {
            return Items.Sum(x => x.Quantity * x.Price);
        }
    }
}
