﻿namespace SanalMarketAPI.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }
        public ApplicationUser User { get; set; }
    }
}
