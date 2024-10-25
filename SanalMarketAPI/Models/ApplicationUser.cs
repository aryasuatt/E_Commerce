using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SanalMarketAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public bool IsSeller { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }

        // Satıcıya ait olan siparişler
        public ICollection<Order> CustomerOrders { get; set; } // Müşteri olduğu siparişler
        public ICollection<Order> SellerOrders { get; set; }   // Satıcı olduğu siparişler
        
    }
}
