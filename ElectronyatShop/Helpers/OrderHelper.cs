using ElectronyatShop.Data;
using ElectronyatShop.Enums;
using ElectronyatShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Helpers;

public static class OrderHelper
{
    public static Order ConvertDtoToOrder(int cartId, ApplicationDbContext context, List<OrderItem> orderItems)
    {
        var cart = context.Carts.Include(c => c.CartItems).First(c => c.Id == cartId);
        var user = context.Users.Find(cart.UserId);
        var order = new Order
        {
            UserId = cart.UserId,
            CreateDate = DateTime.Now,
            Address = user.Address,
            Status = OrderStatus.Ordered,
        };
        foreach (var item in cart.CartItems)
        {
            var product = context.Products.Find(item.ProductId);
            order.TotalPrice += (item.Quantity * (product?.ActualPrice ?? 0));
            orderItems.Add(new OrderItem() { ProductId = item.ProductId, Quantity = item.Quantity });
        }
        context.Orders.Add(order);
        context.SaveChanges();
        return order;
    }

    public static void ClearCart(Cart cart, ApplicationDbContext context)
    {
        cart.CartItems?.Clear();
        cart.TotalPrice = 0;
        context.Carts.Update(cart);
        context.SaveChanges();
    }
    
    public static void AssignItemsIntoOrder(Order order, List<OrderItem> orderItems, ApplicationDbContext context)
    {
        foreach (var item in orderItems)
        {
            item.OrderId = order.Id;
            context.OrderItems.Add(item);
        }
        context.SaveChanges();
    }
}
