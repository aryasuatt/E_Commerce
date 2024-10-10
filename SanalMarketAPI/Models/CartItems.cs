namespace SanalMarketAPI.Models
{
    public class CartItems
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        //Navigation property to link to the Product
        public virtual Product ?Product { get; set; } // This will link to the Product class

        // Foreign Key for Customer
        public string CustomerId { get; set; }
        public virtual ApplicationUser Customer { get; set; } // ApplicationUser ile ilişkilendirme
    }
}
