using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.BasketModels
{
    public class CustomerBasket
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ICollection<BasketItem> Items { get; set; } = new HashSet<BasketItem>();

    }
}
