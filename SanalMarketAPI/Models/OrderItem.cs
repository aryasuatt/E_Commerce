﻿namespace SanalMarketAPI.Models
{
    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}