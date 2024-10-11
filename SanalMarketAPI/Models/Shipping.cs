using Microsoft.EntityFrameworkCore;
namespace SanalMarketAPI.Models
{
    public class Shipping
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; } // e.g., DHL, UPS
        public DateTime ShippedDate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public string Status { get; set; } // e.g., Shipped, In Transit, Delivered

        public int OrderId { get; set; }
        public Order Order { get; set; }  //bire-bir ilişki
    }
}
