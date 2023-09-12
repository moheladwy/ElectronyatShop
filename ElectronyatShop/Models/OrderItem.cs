using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronyatShop.Models
{
	public class OrderItem
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Order")]
		public int? OrderId { get; set; }

		public virtual Order? Order { get; set; }

		public int? ProductId { get; set; }

		public virtual Product? Product { get; set; }

		[Required]
		public int Quantity { get; set; }
	}
}
