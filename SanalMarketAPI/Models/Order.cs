using System;
using System.Collections.Generic;

namespace SanalMarketAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int ShippingId { get; set; }

        public string CustomerId { get; set; } // Foreign key olarak CustomerId
        public ApplicationUser Customer { get; set; } // Müşteri için navigation property

        public string SellerId { get; set; } // Foreign key olarak SellerId
        public ApplicationUser Seller { get; set; } // Satıcı için navigation property

        public List<Shipping> Shippings { get; set; } // İlgili kargolar
        public List<OrderItem> OrderItems { get; set; } // Sipariş kalemleri
    }
}
