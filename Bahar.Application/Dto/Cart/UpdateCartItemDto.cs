using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahar.Application.Dto.Cart
{
    public class UpdateCartItemDto
    {
        public long CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
