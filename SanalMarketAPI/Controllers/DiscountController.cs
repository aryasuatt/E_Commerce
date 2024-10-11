using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanalMarketAPI.Data;
using SanalMarketAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SanalMarketAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : Controller
    {
        private readonly AppDbContext _context;

        public DiscountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscounts()
        {
            var discounts = await _context.Discounts
                                          .Where(d => d.IsActive)
                                          .ToListAsync();
            return Ok(discounts);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount([FromBody] Discount discount)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
            return Ok(discount);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveDiscount(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null) return NotFound();

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
