using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.OrderModels
{
    public class Order : BaseEntity<Guid>
    {
        public string BuyerEmail { get; set; } = string.Empty;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public ShippingAddress ShipToAddress { get; set; } = new ShippingAddress();

        public int DeliveryMethodId { get; set; } //FK
        public DeliveryMethod? DeliveryMethod { get; set; }

        public decimal Subtotal { get; set; }

        // computed or saved
        public decimal GetTotal() => Subtotal + (DeliveryMethod?.Cost ?? 0m);

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
