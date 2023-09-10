using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronyatShop.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal TotalPrice { get; set; } = 0;
        
        [ForeignKey("User")]
        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }

        public ICollection<CartItem>? CartItems { get; set; }
    }
}
