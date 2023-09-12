using ElectronyatShop.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronyatShop.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }

		public DateTime CreateDate { get; set; }

		public decimal TotalPrice { get; set; }

		public string Address { get; set; }

		public OrderStatus Status { get; set; } = OrderStatus.Ordered;

		[ForeignKey("User")]
		public string? UserId { get; set; }

		public virtual ApplicationUser? User { get; set; }

		public ICollection<OrderItem>? OrderItems { get; set; }
	}
}
