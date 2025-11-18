using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.BasketModels
{
    public class BasketItem
    {
        public int Id { get; set; }

        public int ProductName { get; set; }

        public string PictureUrl { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
