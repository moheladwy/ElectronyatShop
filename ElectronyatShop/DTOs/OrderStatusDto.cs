// ElectronyatShopElectronyatShopWebAPIOrderStatusDto.cs
// 20241283:39
// eladwyeladwy

using ElectronyatShop.Enums;

namespace ElectronyatShop.DTOs;

public class OrderStatusDto
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
}