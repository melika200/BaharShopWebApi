using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bahar.Domain;

namespace Bahar.Application.InterfaceRepository
{
    public interface ICartRepository
    {
        IQueryable<Cart> GetAll(Expression<Func<Cart, bool>> where = null);
        Task<Cart> GetById(int id);

        Task Add(Cart cart);
        Task Update(Cart cart);
        Task Delete(int id);
        Task Delete(Cart cart);
        IQueryable<CartItem> GetAllCartItems(Expression<Func<CartItem, bool>> where = null);
        Task AddCartItem(CartItem item);
        Task DeleteCartItem(CartItem item);
        Task UpdateCartItem(CartItem item);
    }
}
