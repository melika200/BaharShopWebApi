using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahar.Application.Dto.Product
{
    public class PagedProductDto
    {
        public List<ProductDto> Items { get; set; }
        public int Page { get; set; }
        public int TotalPage { get; set; }
    }

}
