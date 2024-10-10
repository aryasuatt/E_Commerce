using Microsoft.AspNetCore.Identity;
namespace SanalMarketAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsSeller { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
