using Bahar.Application.InterfaceRepository;
using Bahar.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bahar.Application.Services
{
    public class CartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> AddToCart(long userId, long productId, int quantity)
        {
            var product = await _productRepository.GetById(productId);
            if (product == null || product.Stock < quantity)
                return false;

            product.Stock -= quantity;
            await _productRepository.Update(product);

            var cart = await _cartRepository.GetAll(c => c.UserId == userId && c.Status == Domain.Enum.Status.Created)
                                            .Include(c => c.Items)
                                            .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Status = Domain.Enum.Status.Created,
                    Address = "",
                    Mobile = "",
                    Payed = default
                };
                await _cartRepository.Add(cart);
            }
        
            var cartItem = cart.Items?.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                cartItem.Price = cartItem.Quantity * product.Price;
                await _cartRepository.UpdateCartItem(cartItem);
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price * quantity
                };
                await _cartRepository.AddCartItem(cartItem);
            }

            return true;
        }
      
        public async Task<List<CartItem>> GetUserCartItems(long userId)
        {
         
            var items = await _cartRepository.GetAllCartItems(ci => ci.ShoppingCart.UserId == userId && ci.ShoppingCart.Status == Domain.Enum.Status.Created)
                                            .Include(ci => ci.Product)
                                            .Include(ci => ci.ShoppingCart)
                                            .ToListAsync();
            return items;
        }

        public async Task<bool> RemoveCartItem(long cartItemId)
        {
            var item = await _cartRepository.GetAllCartItems(ci => ci.CartId == cartItemId)
                                            .Include(ci => ci.Product)
                                            .FirstOrDefaultAsync();
            if (item == null)
                return false;

            item.Product.Stock += item.Quantity;
            await _productRepository.Update(item.Product);

            await _cartRepository.DeleteCartItem(item);
            return true;
        }

        public async Task<bool> UpdateCartItemQuantity(long cartItemId, int quantity)
        {
            var item = await _cartRepository.GetAllCartItems(ci => ci.CartId == cartItemId)
                                            .Include(ci => ci.Product)
                                            .FirstOrDefaultAsync();


            if (item == null || item.Product.Stock + item.Quantity < quantity)
                return false;
          
            int diff = quantity - item.Quantity;
            item.Product.Stock -= diff;
            await _productRepository.Update(item.Product);

            item.Quantity = quantity;
            item.Price = item.Product.Price * quantity;
            await _cartRepository.UpdateCartItem(item);

            return true;
        }

        public async Task<bool> Checkout(long userId, string address, string mobile)
        {
            var cart = await _cartRepository.GetAll(c => c.UserId == userId && c.Status == Domain.Enum.Status.Created)
                                            .FirstOrDefaultAsync();
            if (cart == null)
                return false;

            cart.Address = address;
            cart.Mobile = mobile;
            cart.Status = Domain.Enum.Status.Final;
            cart.Payed = DateTime.Now;

            await _cartRepository.Update(cart);
            return true;
        }
    }
}