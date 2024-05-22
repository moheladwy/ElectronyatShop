using System.ComponentModel.DataAnnotations;

namespace ElectronyatShop.ViewModels
{
	public class CartItemViewModel
	{
		public int Id { get; set; }

		[Required]
		public int Quantity { get; set; } = 0;

		public int? CartId { get; set; }

		public int? ProductId { get; set; }
	}
}
