namespace SanalMarketAPI.Models
{
    public class User
    {
        public string Id { get; set; } //Primary key
        public string? FullName { get; set; }
        public bool IsSeller { get; set; }  // True if user is a seller
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }

    }
}
