namespace SanalMarketAPI.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; } // Hangi siparişe ait olduğu
        public Order Order { get; set; }

        public int ProductId { get; set; } // Sipariş edilen ürün
        public Product Product { get; set; }

        public int Quantity { get; set; } // Sipariş edilen miktar
        public decimal Price { get; set; } // Birim fiyatı
    }
}
