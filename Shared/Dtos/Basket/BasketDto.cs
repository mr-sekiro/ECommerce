using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Basket
{
    public class BasketDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ICollection<BasketItemDto> Items { get; set; } = new HashSet<BasketItemDto>();
    }
}
