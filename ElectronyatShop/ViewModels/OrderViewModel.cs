using ElectronyatShop.Enums;
using ElectronyatShop.Models;
using Microsoft.Build.Framework;

namespace ElectronyatShop.ViewModels
{
	public class OrderViewModel
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public DateTime CreateDate { get; set; }

		[Required]
        public OrderStatus Status { get; set; }

		public const decimal ShippingCost = 10;

		public decimal SubTotal { get; set; }

		public decimal TotalPrice => SubTotal + ShippingCost;

		public List<OrderItem>? Items { get; set; }

    }
}
