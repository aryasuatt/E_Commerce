using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Linq; 
using System.Security.Claims;
using SanalMarketAPI.Data;
using SanalMarketAPI.Models;

namespace SanalMarketAPI.Controllers
{
    [Authorize(Roles = "Admin, Seller")] // Tüm metotlar için yetkilendirme
    public class SellerController : Controller
    {
        private readonly AppDbContext _context;

        public SellerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult SellerOrders()
        {
            // Giriş yapan kullanıcıyı al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kullanıcı satıcı mı kontrol et
            var seller = _context.Users
                                 .Include(u => u.Orders)
                                     .ThenInclude(o => o.OrderItems)
                                         .ThenInclude(oi => oi.Product)
                                 .FirstOrDefault(u => u.Id == userId && u.IsSeller);

            if (seller == null || seller.Orders == null)
            {
                return NotFound("Satıcıya ait sipariş bulunamadı.");
            }

            // Satıcıya ait olan siparişleri getir
            var orders = seller.Orders.ToList(); // Siparişleri listele

            return View(orders); // View'e gönder
        }
    }
}
