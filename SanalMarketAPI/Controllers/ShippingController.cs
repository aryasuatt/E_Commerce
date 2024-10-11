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
    public class ShippingController : Controller
    {
        private readonly AppDbContext _context;

        public ShippingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetShippingInfo(int orderId)
        {
            var shipping = await _context.Shippings
                                         .FirstOrDefaultAsync(s => s.OrderId == orderId);
            if (shipping == null) return NotFound();
            return Ok(shipping);
        }

        [HttpPost]
        public async Task<IActionResult> AddShippingInfo([FromBody] Shipping shipping)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Shippings.Add(shipping);
            await _context.SaveChangesAsync();
            return Ok(shipping);
        }

    }
}
