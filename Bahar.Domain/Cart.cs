using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bahar.Domain.Enum;

namespace Bahar.Domain
{
    public class Cart
    {
        public long Id { get; set; }
        public DateTime Payed { get; set; }
        public long UserId { get; set; }
        public string Address { get; set; } 
        public string Mobile { get; set; }
        public Status Status { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<CartItem> Items { get; set; }
    }
}
