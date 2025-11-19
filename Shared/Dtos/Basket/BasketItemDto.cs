using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Basket
{
    public class BasketItemDto
    {
        public int Id { get; set; }

        public int ProductName { get; set; }

        public string PictureUrl { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
        [Range(1,double.MaxValue)]
        public decimal Price { get; set; }
    }
}
