using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Core.Entities
{
    public class ShoppingCartItem {
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
        public string? ProductId { get; set; }
        public string? ImageFile { get; set; }
        public string?  ProductName { get; set; }
    }
}
