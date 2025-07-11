using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bahar.Domain;

namespace Bahar.Application.Dto.Product
{
    public class ProductDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        public bool IsAvailable { get; set; }
        public long CategoryId { get; set; }
        public double AverageRating { get; set; }

    }
}
