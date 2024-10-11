namespace SanalMarketAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public User Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        // Shipping ile olan ilişki
        public int ShippingId { get; set; }  // Foreign Key
        public ICollection<Shipping> Shipping { get; set; } = new List<Shipping>(); // Koleksiyon olarak tanımlanmalı
    }
}
