using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ElectronyatShop.Models;
using System.Collections;

namespace ElectronyatShop.ViewModels
{
    public class CartViewModel
    {
        public int Id { get; set; }

        public const int ShippingCost = 10;


		[DisplayFormat(DataFormatString = "{0.00}")]
		public decimal SubTotalPrice { get; set; } = 0;


		[DisplayFormat(DataFormatString = "{0.00}")]
		public decimal TotalPrice => SubTotalPrice + ShippingCost;

        public string? UserId { get; set; }

        public List<CartItem>? CartItems { get; set; }
    }
}
