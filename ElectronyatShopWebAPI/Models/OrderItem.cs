// ElectronyatShopElectronyatShopWebAPIOrderItem.cs
// 202412720:49
// eladwyeladwy

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronyatShopWebAPI.Models;


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
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than 0!")]
    public int Quantity { get; set; }
}