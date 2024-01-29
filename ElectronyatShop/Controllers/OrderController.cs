using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronyatShop.Data;
using ElectronyatShop.DTOs;
using ElectronyatShop.Helpers;
using ElectronyatShop.Models;

namespace ElectronyatShop.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize("CustomerRole")]
public class OrderController : ControllerBase
{
    #region Controller Constructor and Attributes

    private readonly ApplicationDbContext context;

    public OrderController(ApplicationDbContext context) => this.context = context;

    #endregion
    
    #region Controller Endpoints
    
    [HttpGet("{id}")]
    [Route("get-all/{id}")]
    public IActionResult GetAllOrders([FromRoute] string id)
    {
        var orders = context.Orders.Where(o => o.UserId == id).ToList();
        return Ok(orders);
    }
    
    [HttpGet]
    [Route("get-by-id/{userId}/{orderId}")]
    public IActionResult GetOrderById([FromRoute] string userId, [FromRoute] int orderId)
    {
        var order = context.Orders.Find(orderId);
        if (order == null)
            return NotFound($"Order with id = {orderId} Not Found");
        return Ok(order);
    }
    
    [HttpPost]
    [Route("create")]
    public IActionResult CreateOrder([FromBody] int cartId)
    {
        List<OrderItem> orderItems = [];
        var cart = context.Carts.Include(c => c.CartItems).First(c => c.Id == cartId);
        var order = OrderHelper.ConvertDtoToOrder(cartId, context, orderItems);
        OrderHelper.ClearCart(cart, context);
        OrderHelper.AssignItemsIntoOrder(order, orderItems, context);
        return Ok(order);
    }
    
    [HttpPut]
    [Route("update-status")]
    public IActionResult UpdateOrderStatus([FromBody] OrderStatusDto order)
    {
        var orderToUpdate = context.Orders.Find(order.OrderId);
        if (orderToUpdate == null)
            return NotFound($"Order with id = {order.OrderId} Not Found");
        orderToUpdate.Status = order.Status;
        context.Orders.Update(orderToUpdate);
        context.SaveChanges();
        return Ok(orderToUpdate);
    }
    
    [HttpDelete]
    [Route("delete/{id}")]
    public IActionResult DeleteOrder([FromRoute] int id)
    {
        var order = context.Orders.Find(id);
        if (order == null)
            return NotFound($"Order with id = {id} Not Found");
        context.Orders.Remove(order);
        context.SaveChanges();
        return Ok();
    }
    
    #endregion
}

#region Old MVC Controller

// using ElectronyatShop.Data;
// using ElectronyatShop.Enums;
// using ElectronyatShop.Models;
// using ElectronyatShop.ViewModels;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
//
// namespace ElectronyatShop.Controllers
// {
// 	[Authorize("CustomerRole")]
// 	public class OrderController : Controller
// 	{
// 		#region Controller Constructor and Attributes
//
// 		private ApplicationDbContext Context;
//
// 		private UserManager<ApplicationUser> UserManager;
//
// 		private string userId;
//
// 		public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) 
// 		{
// 			Context = context;
// 			UserManager = userManager;
// 		}
//
// 		#endregion
//
// 		#region Controller Actions
//
// 		[HttpGet]
// 		public IActionResult Index()
// 		{
// 			SetUser();
// 			List<Order> orders = Context.Orders.Include(o => o.OrderItems).Where(o => o.UserId == userId).ToList();
// 			return View("Index", orders);
// 		}
//
// 		[HttpPost]
// 		public IActionResult Create([FromForm] int CartId)
// 		{
// 			Cart? cart = Context.Carts.Include(c => c.CartItems).Where(c => c.Id == CartId).FirstOrDefault();
// 			if (cart != null)
// 			{
// 				List<CartItem> cartItems = cart.CartItems.ToList();
// 				List<OrderItem> orderItems = new();
// 				Order order = CreateOrder(cart, orderItems);
//
// 				ClearCart(cart);
//
// 				CreateOrderItems(order, orderItems);
// 			}
// 			return RedirectToAction("Index");
// 		}
//
// 		[HttpGet]
// 		public IActionResult Details([FromRoute] int Id)
// 		{
// 			Order? order = Context.Orders.Include(o => o.OrderItems).Where(o => o.Id == Id).FirstOrDefault();
// 			if (order != null)
// 			{
// 				OrderViewModel model = new()
// 				{
// 					Id = Id,
// 					CreateDate = order.CreateDate,
// 					Status = order.Status,
// 					SubTotal = order.TotalPrice - OrderViewModel.ShippingCost,
// 					Items = order.OrderItems.ToList()
// 				};
// 				return View("Details", model);
// 			}
// 			return RedirectToAction("Index");
//         }
//
// 		[HttpGet]
// 		public IActionResult Cancel([FromRoute] int Id)
// 		{
// 			Order? order = Context.Orders.Include(o => o.OrderItems).Where(o => o.Id == Id).FirstOrDefault();
// 			if (order != null)
// 			{
// 				foreach (var item in order.OrderItems)
// 				{
// 					Product? product = Context.Products.Find(item.ProductId);
// 					if (product != null)
// 					{
// 						product.AvailableQuantity += item.Quantity;
// 						Context.Products.Update(product);
// 						Context.SaveChanges();
// 					}
// 				}
// 				order.Status = OrderStatus.Cancelled;
// 				Context.Orders.Update(order);
// 				Context.SaveChanges();
// 			}
// 			return RedirectToAction("Index");
// 		}
//
// 		#endregion
//
// 		#region Controller Logic
//
// 		private void SetUser() => userId = UserManager.GetUserId(User);
//
// 		private Order CreateOrder(Cart cart, List<OrderItem> orderItems)
// 		{
// 			List<CartItem> cartItems = cart.CartItems.ToList();
// 			string? address = Context.Users.Find(cart.UserId)?.Address;
// 			Order order = new()
// 			{
// 				CreateDate = DateTime.Now,
// 				Address = address,
// 				Status = OrderStatus.Ordered,
// 				UserId = cart?.UserId,
// 			};
// 			foreach (CartItem item in cart.CartItems)
// 			{
// 				Product? product = Context.Products.Find(item.ProductId);
// 				order.TotalPrice += (item.Quantity * (product?.ActualPrice ?? 0));
// 				orderItems.Add(new OrderItem() { ProductId = item.ProductId, Quantity = item.Quantity });
// 			}
// 			Context.Orders.Add(order);
// 			Context.SaveChanges();
// 			return order;
// 		}
//
// 		private void CreateOrderItems(Order order, List<OrderItem> orderItems)
// 		{
// 			foreach (var item in orderItems)
// 			{
// 				item.OrderId = order.Id;
// 				Context.OrderItems.Add(item);
// 			}
// 			Context.SaveChanges();
// 		}
//
// 		private void ClearCart(Cart cart)
// 		{
// 			cart.CartItems.Clear();
// 			cart.TotalPrice = 0;
// 			Context.Carts.Update(cart);
// 			Context.SaveChanges();
// 		}
//
// 		#endregion
// 	}
// }

#endregion