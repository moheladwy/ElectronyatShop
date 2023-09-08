using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronyatShop.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("User")]
        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }

        [ForeignKey("Products")]
        public int? ProductId { get; set; }

        public virtual Product? Products { get; set;}
    }
}
