using Microsoft.AspNetCore.Mvc;
using SanalMarketAPI.Data;
using SanalMarketAPI.Models;

namespace SanalMarketAPI.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly AppDbContext _context;

        public PaymentsController(AppDbContext context)
        {
            _context = context;
        }

        // Process a payment
        [HttpPost]
        public async Task<ActionResult<Payment>> ProcessPayment(Payment payment)
        {
            var order = await _context.Orders.FindAsync(payment.OrderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ProcessPayment), new { id = payment.Id }, payment);
        }

    }
}
