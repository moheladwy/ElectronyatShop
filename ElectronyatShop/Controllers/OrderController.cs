using ElectronyatShop.Data;
using ElectronyatShop.Enums;
using ElectronyatShop.Models;
using ElectronyatShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Controllers
{
	[Authorize("CustomerRole")]
	public class OrderController : Controller
	{
		#region Controller Constructor and Attributes

		private SqliteDbContext Context;

		private UserManager<ApplicationUser> UserManager;

		private string userId;

		public OrderController(SqliteDbContext context, UserManager<ApplicationUser> userManager)
		{
			Context = context;
			UserManager = userManager;
		}

		#endregion

		#region Controller Actions

		[HttpGet]
		public IActionResult Index()
		{
			SetUser();
			List<Order> orders = Context.Orders.Include(o => o.OrderItems).Where(o => o.UserId == userId).ToList();
			return View("Index", orders);
		}

		[HttpPost]
		public IActionResult Create([FromForm] int CartId)
		{
			Cart? cart = Context.Carts.Include(c => c.CartItems).Where(c => c.Id == CartId).FirstOrDefault();
			if (cart != null)
			{
				List<CartItem> cartItems = cart.CartItems.ToList();
				List<OrderItem> orderItems = new();
				Order order = CreateOrder(cart, orderItems);

				ClearCart(cart);

				CreateOrderItems(order, orderItems);
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Details([FromRoute] int Id)
		{
			Order? order = Context.Orders.Include(o => o.OrderItems).Where(o => o.Id == Id).FirstOrDefault();
			if (order != null)
			{
				OrderViewModel model = new()
				{
					Id = Id,
					CreateDate = order.CreateDate,
					Status = order.Status,
					SubTotal = order.TotalPrice - OrderViewModel.ShippingCost,
					Items = order.OrderItems.ToList()
				};
				return View("Details", model);
			}
			return RedirectToAction("Index");
        }

		[HttpGet]
		public IActionResult Cancel([FromRoute] int Id)
		{
			Order? order = Context.Orders.Include(o => o.OrderItems).Where(o => o.Id == Id).FirstOrDefault();
			if (order != null)
			{
				foreach (var item in order.OrderItems)
				{
					Product? product = Context.Products.Find(item.ProductId);
					if (product != null)
					{
						product.AvailableQuantity += item.Quantity;
						Context.Products.Update(product);
						Context.SaveChanges();
					}
				}
				order.Status = OrderStatus.Cancelled;
				Context.Orders.Update(order);
				Context.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		#endregion

		#region Controller Logic

		private void SetUser() => userId = UserManager.GetUserId(User);

		private Order CreateOrder(Cart cart, List<OrderItem> orderItems)
		{
			List<CartItem> cartItems = cart.CartItems.ToList();
			string? address = Context.Users.Find(cart.UserId)?.Address;
			Order order = new()
			{
				CreateDate = DateTime.Now,
				Address = address,
				Status = OrderStatus.Ordered,
				UserId = cart?.UserId,
			};
			foreach (CartItem item in cart.CartItems)
			{
				Product? product = Context.Products.Find(item.ProductId);
				order.TotalPrice += (item.Quantity * (product?.ActualPrice ?? 0));
				orderItems.Add(new OrderItem() { ProductId = item.ProductId, Quantity = item.Quantity });
			}
			Context.Orders.Add(order);
			Context.SaveChanges();
			return order;
		}

		private void CreateOrderItems(Order order, List<OrderItem> orderItems)
		{
			foreach (var item in orderItems)
			{
				item.OrderId = order.Id;
				Context.OrderItems.Add(item);
			}
			Context.SaveChanges();
		}

		private void ClearCart(Cart cart)
		{
			cart.CartItems.Clear();
			cart.TotalPrice = 0;
			Context.Carts.Update(cart);
			Context.SaveChanges();
		}

		#endregion
	}
}
