// ElectronyatShopElectronyatShopWebAPIOrder.cs
// 202412720:47
// eladwyeladwy

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ElectronyatShopWebAPI.Enums;

namespace ElectronyatShopWebAPI.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime CreateDate { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Ordered;

    [ForeignKey("User")]
    public string? UserId { get; set; }

    public virtual ApplicationUser? User { get; set; }

    public ICollection<OrderItem>? OrderItems { get; set; }
}