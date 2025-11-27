using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.OrderModels
{
    public class OrderItem : BaseEntity<int>
    {
        public ProductItemOrdered ItemOrdered { get; set; } = new ProductItemOrdered();

        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // FK
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
