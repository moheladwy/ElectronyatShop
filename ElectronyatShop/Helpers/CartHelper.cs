using ElectronyatShop.Data;
using ElectronyatShop.DTOs;
using ElectronyatShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Helpers;

public static class CartHelper
{
    public static CartDto ConvertCartToCartDto(Cart cart, ApplicationDbContext context)
    {
        var dto = new CartDto()
        {
            Id = cart.Id,
            SubTotalPrice = 0,
            CartItems = cart.CartItems.ToList()
        };
        foreach (var item in cart.CartItems)
        {
            var product = context.Products.Find(item.ProductId);
            if (product != null)
                dto.SubTotalPrice += (product.ActualPrice * item.Quantity);
        }
        return dto;
    }

    public static CartItem ConvertCartItemDtoToCartItem(CartItemDto cartItem, ApplicationDbContext context)
    {
        var cart = context.Carts.Include(c => c.CartItems).First(c => c.UserId == cartItem.UserId);
        
        var item = new CartItem()
        {
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity,
            CartId = cart.Id
        };
        
        return item;
    }
}