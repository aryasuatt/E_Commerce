namespace SanalMarketAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } // Rating from 1 to 5
        public DateTime ReviewDate { get; set; }

        public Product Product { get; set; }
        public ApplicationUser User { get; set; }
    }
}
