using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ElectronyatShop.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; } = 0;

        [ForeignKey("cart")]
        public int? CartId { get; set; }

        public virtual Cart? cart { get; set; }

		[ForeignKey("Product")]
		public int? ProductId { get; set; }

		public virtual Product? Product { get; set; }
	}
}
