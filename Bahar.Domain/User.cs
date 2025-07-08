using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bahar.Domain
{
   public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        public ICollection<Cart> Carts { get; set; }
    }
}
