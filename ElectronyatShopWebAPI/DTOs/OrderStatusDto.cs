// ElectronyatShopElectronyatShopWebAPIOrderStatusDto.cs
// 20241283:39
// eladwyeladwy

using ElectronyatShopWebAPI.Enums;

namespace ElectronyatShopWebAPI.DTOs;

public class OrderStatusDto
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
}