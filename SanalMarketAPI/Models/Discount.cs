namespace SanalMarketAPI.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string Code { get; set; } // Discount code
        public decimal Percentage { get; set; } // Percentage discount
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; } // Products that this discount applies to

    }
}
