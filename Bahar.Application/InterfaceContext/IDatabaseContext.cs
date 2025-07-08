using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bahar.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bahar.Application.InterfaceContext
{
    public interface IDatabaseContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<User> Users { get; set; }

        DbSet<Cart> Carts { get; set; }
        DbSet<CartItem> CartItems { get; set; }

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
