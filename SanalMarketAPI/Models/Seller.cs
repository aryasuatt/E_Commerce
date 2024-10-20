namespace SanalMarketAPI.Models
{
    public class Seller
    {
        public int SellerId { get; set; }
        public string SellerName { get; set; }

        public List<Product> Products { get; set; } // Satıcının sahip olduğu ürünler
    }
}
