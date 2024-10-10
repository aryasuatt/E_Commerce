using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SanalMarketAPI.Data;
using SanalMarketAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace SanalMarketAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public CartController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Get the current user's cart
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<CartItems>>> GetCartItems()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.CartItems
                                 .Where(c => c.CustomerId == userId)
                                 .Include(c => c.Product)
                                 .ToListAsync();
        }

        // Add item to cart
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<CartItems>> AddToCart(int productId, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.CustomerId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity; // Update quantity if the item is already in the cart
            }
            else
            {
                cartItem = new CartItems
                {
                    ProductId = productId,
                    CustomerId = userId,
                    Quantity = quantity
                };

                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCartItems), new { id = cartItem.Id }, cartItem);
        }

        // Update cart item quantity
        [HttpPut("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateCartItem(int id, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null || cartItem.CustomerId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Remove item from cart
        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null || cartItem.CustomerId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
