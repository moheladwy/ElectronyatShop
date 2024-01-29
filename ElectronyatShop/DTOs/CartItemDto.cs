namespace ElectronyatShop.DTOs;

public class CartItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string? UserId { get; set; }
}