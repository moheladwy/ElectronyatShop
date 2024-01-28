using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronyatShopWebAPI.Models;

public class CartItem
{
    [Key]
    public int Id { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or more!")]
    public int Quantity { get; set; } = 0;

    [ForeignKey("cart")]
    public int? CartId { get; set; }

    public virtual Cart? cart { get; set; }

    [ForeignKey("Product")]
    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}