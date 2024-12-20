using ElectronyatShop.Data;
using ElectronyatShop.Enums;
using ElectronyatShop.Models;
using ElectronyatShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Controllers;

[Authorize("CustomerRole")]
public class OrderController(SqliteDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
	#region Controller Constructor and Attributes

	private string _userId = null!;

	#endregion

	#region Controller Actions

	[HttpGet]
	public async Task<IActionResult> Index()
	{
		SetUser();
		var orders = await context.Orders
			.Include(o => o.OrderItems)
			.Where(o => o.UserId == _userId)
			.AsNoTracking()
			.ToListAsync();
		return View("Index", orders);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromForm] int cartId)
	{
		var cart = await context.Carts
			.Include(c => c.CartItems)
			.FirstOrDefaultAsync(c => c.Id == cartId);

		if (cart is null) return RedirectToAction("Index");

		var orderItems = new List<OrderItem>();
		var order = await CreateOrder(cart, orderItems);
		await ClearCart(cart);
		await CreateOrderItems(order, orderItems);

		return RedirectToAction("Index");
	}

	[HttpGet]
	public async Task<IActionResult> Details([FromRoute] int id)
	{
		var order = await context.Orders
			.Include(o => o.OrderItems)
			.FirstOrDefaultAsync(o => o.Id == id);

		if (order is null) return RedirectToAction("Index");
		var model = new OrderViewModel
		{
			Id = id,
			CreateDate = order.CreateDate,
			Status = order.Status,
			SubTotal = order.TotalPrice - OrderViewModel.ShippingCost,
			Items = order.OrderItems?.ToList() ?? throw new Exception("Order Items are not found")
		};
		return View("Details", model);
	}

	[HttpGet]
	public async Task<IActionResult> Cancel([FromRoute] int id)
	{
		var order = await context.Orders
			.Include(o => o.OrderItems)
			.FirstOrDefaultAsync(o => o.Id == id);

		if (order is null) return RedirectToAction("Index");
		foreach (var item in order.OrderItems ?? [])
		{
			var product = await context.Products.FindAsync(item.ProductId);
			if (product is null) continue;

			product.AvailableQuantity += item.Quantity;
			context.Products.Update(product);
		}
		order.Status = OrderStatus.Cancelled;
		context.Orders.Update(order);
		await context.SaveChangesAsync();
		return RedirectToAction("Index");
	}

	#endregion

	#region Controller Logic

	private void SetUser() => _userId = userManager.GetUserId(User) ?? throw new ArgumentNullException(_userId, "User is not authenticated");

	private async Task<Order> CreateOrder(Cart cart, List<OrderItem> orderItems)
	{
		var cartItems = cart.CartItems?.ToList() ?? throw new Exception("Cart Items are not found");
		var address = (await context.Users.FindAsync(cart.UserId))?.Address;
		Order order = new()
		{
			CreateDate = DateTime.Now,
			Address = address ?? string.Empty,
			Status = OrderStatus.Ordered,
			UserId = cart.UserId,
		};
		foreach (var item in cartItems)
		{
			var product = await context.Products.FindAsync(item.ProductId);
			order.TotalPrice += item.Quantity * (product?.ActualPrice ?? 0);
			orderItems.Add(new OrderItem { ProductId = item.ProductId, Quantity = item.Quantity });
		}
		await context.Orders.AddAsync(order);
		await context.SaveChangesAsync();
		return order;
	}

	private async Task CreateOrderItems(Order order, List<OrderItem> orderItems)
	{
		foreach (var item in orderItems)
		{
			item.OrderId = order.Id;
			await context.OrderItems.AddAsync(item);
		}
		await context.SaveChangesAsync();
	}

	private async Task ClearCart(Cart cart)
	{
		cart.CartItems?.Clear();
		cart.TotalPrice = 0;
		context.Carts.Update(cart);
		await context.SaveChangesAsync();
	}

	#endregion
}