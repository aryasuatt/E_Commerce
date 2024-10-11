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
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetReviews(int productId)
        {
            var reviews = await _context.Reviews
                                        .Where(r => r.ProductId == productId)
                                        .ToListAsync();
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok(review);
        }

    }
}
