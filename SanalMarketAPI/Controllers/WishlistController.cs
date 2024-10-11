using Microsoft.AspNetCore.Mvc;
using SanalMarketAPI.Data;
using SanalMarketAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SanalMarketAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;

        public WishlistController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishlist(string userId)
        {
            var wishlist = await _context.Wishlists
                                         .Where(w => w.UserId == userId)
                                         .Include(w => w.Product)
                                         .ToListAsync();
            return Ok(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] Wishlist wishlist)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return Ok(wishlist);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromWishlist(int id)
        {
            var wishlistItem = await _context.Wishlists.FindAsync(id);
            if (wishlistItem == null) return NotFound();

            _context.Wishlists.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
