using ElectronyatShopWebAPI.Data;
using ElectronyatShopWebAPI.DTOs;
using ElectronyatShopWebAPI.Helpers;
using ElectronyatShopWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShopWebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize("CustomerRole")]
public class OrderController : ControllerBase
{
    #region Controller Constructor and Attributes

    private ApplicationDbContext Context;

    public OrderController(ApplicationDbContext context) 
    {
        Context = context;
    }

    #endregion
    
    #region Controller Endpoints
    
    [HttpGet("{id}")]
    [Route("get-all/{id}")]
    public IActionResult GetAllOrders([FromRoute] string id)
    {
        var orders = Context.Orders.Where(o => o.UserId == id).ToList();
        return Ok(orders);
    }
    
    [HttpGet]
    [Route("get-by-id/{userId}/{orderId}")]
    public IActionResult GetOrderById([FromRoute] string userId, [FromRoute] int orderId)
    {
        var order = Context.Orders.Find(orderId);
        if (order == null)
            return NotFound($"Order with id = {orderId} Not Found");
        return Ok(order);
    }
    
    [HttpPost]
    [Route("create")]
    public IActionResult CreateOrder([FromBody] int cartId)
    {
        List<OrderItem> orderItems = [];
        var cart = Context.Carts.Include(c => c.CartItems).First(c => c.Id == cartId);
        var order = OrderHelper.ConvertDtoToOrder(cartId, Context, orderItems);
        OrderHelper.ClearCart(cart, Context);
        OrderHelper.AssignItemsIntoOrder(order, orderItems, Context);
        return Ok(order);
    }
    
    [HttpPut]
    [Route("update-status")]
    public IActionResult UpdateOrderStatus([FromBody] OrderStatusDto order)
    {
        var orderToUpdate = Context.Orders.Find(order.OrderId);
        if (orderToUpdate == null)
            return NotFound($"Order with id = {order.OrderId} Not Found");
        orderToUpdate.Status = order.Status;
        Context.Orders.Update(orderToUpdate);
        Context.SaveChanges();
        return Ok(orderToUpdate);
    }
    
    [HttpDelete]
    [Route("delete/{id}")]
    public IActionResult DeleteOrder([FromRoute] int id)
    {
        var order = Context.Orders.Find(id);
        if (order == null)
            return NotFound($"Order with id = {id} Not Found");
        Context.Orders.Remove(order);
        Context.SaveChanges();
        return Ok();
    }
    
    #endregion
}