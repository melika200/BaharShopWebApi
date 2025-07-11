using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahar.Domain
{
    public class Product 
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        public string ImagePath { get; set; }  

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public long CategoryId { get; set; }

        public Category Category { get; set; }

        public bool IsAvailable { get; set; }
        public int TotalRatings { get; set; } = 0;
        public double AverageRating { get; set; } = 0.0;
        public DateTime InsertTime { get; set; } = DateTime.Now;
    }
}


