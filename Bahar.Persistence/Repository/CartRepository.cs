using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bahar.Application.InterfaceRepository;
using Bahar.Domain;
using Bahar.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bahar.Persistence.Repository
{
    public class CartRepository:ICartRepository
    {
        private readonly BaharDbContext _context;
        public CartRepository(BaharDbContext context)
        {
            _context = context;
        }

        public async Task Add(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task AddCartItem(CartItem item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var cart = await GetById(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Cart cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartItem(CartItem item)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Cart> GetAll(Expression<Func<Cart, bool>> where = null)
        {
            var carts = _context.Carts.AsQueryable();
            if (where != null)
            {
                carts = carts.Where(where);
            }

            return carts;
        }

        public IQueryable<CartItem> GetAllCartItems(Expression<Func<CartItem, bool>> where = null)
        {
            var baskets = _context.CartItems.AsQueryable();
            if (where != null)
            {
                baskets = baskets.Where(where);
            }

            return baskets;
        }

        public async Task<Cart> GetById(int id)
        {
            return await _context.Carts.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Update(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItem(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
