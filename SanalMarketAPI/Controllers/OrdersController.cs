using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanalMarketAPI.Data;
using SanalMarketAPI.Models;

namespace SanalMarketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrdersController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Get all orders for the current user
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Orders
                                 .Where(o => o.CustomerId == userId)
                                 .Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.Product)
                                 .ToListAsync();
        }

        // Place an order
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Order>> PlaceOrder()
        {
            var userId = _userManager.GetUserId(User);

            // Get cart items for the user
            var cartItems = await _context.CartItems
                                          .Where(c => c.CustomerId == userId)
                                          .Include(c => c.Product)
                                          .ToListAsync();

            if (cartItems == null || !cartItems.Any())
            {
                return BadRequest("Cart is empty");
            }

            // Create a new order
            var order = new Order
            {
                CustomerId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity),
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);

            // Clear the user's cart
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
        }

        // Get specific order details
        [HttpGet("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                                      .Include(o => o.OrderItems)
                                      .ThenInclude(oi => oi.Product)
                                      .FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == _userManager.GetUserId(User));

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }
}
