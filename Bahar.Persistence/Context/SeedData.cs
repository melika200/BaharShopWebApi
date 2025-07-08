using System;
using System.Collections.Generic;
using System.Linq;
using Bahar.Domain;
using Bahar.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Bahar.Persistence.Context
{
    public class SeedData
    {
        private readonly BaharDbContext _context;

        public SeedData(BaharDbContext context)
        {
            _context = context;
        }

        public void SeedDatabase()
        {
            if (_context.Categories.Any() || _context.Products.Any() || _context.Users.Any())
                return;

            var categories = new List<Category>
            {
                new Category { Name = "لوازم الکترونیکی" },
                new Category { Name = "کتاب‌ها" },
                new Category { Name = "پوشاک" }
            };
            _context.Categories.AddRange(categories);
            _context.SaveChanges();

            var products = new List<Product>
            {
                new Product
                {
                    Name = "گوشی موبایل",
                    Price = 12000000,
                    Stock = 10,
                    CategoryId = categories[0].Id,
                    Description = "گوشی هوشمند جدید",
                    IsAvailable = true,
                    ImagePath = "Img/mobile.jpg"
                },
                new Product
                {
                    Name = "کتاب آموزش سی‌شارپ",
                    Price = 50000,
                    Stock = 100,
                    CategoryId = categories[1].Id,
                    Description = "کتاب جامع برنامه‌نویسی سی‌شارپ",
                    IsAvailable = true,
                    ImagePath = "Img/book.jpg"
                },
                new Product
                {
                    Name = "پیراهن مردانه",
                    Price = 250000,
                    Stock = 50,
                    CategoryId = categories[2].Id,
                    Description = "پیراهن کتان مردانه",
                    IsAvailable = true,
                    ImagePath = "Img/manClothes.jpg"
                }
            };
            _context.Products.AddRange(products);
            _context.SaveChanges();

            var users = new List<User>
            {
                new User
                {
                    Username = "baharuser",
                    Mobile = "09121234567",
                    Address = "تهران، خیابان انقلاب",
                    PasswordHash = HashPassword("123456"),
                    Email = "baharuser@example.com",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                }
            };
            _context.Users.AddRange(users);
            _context.SaveChanges();

            var cart = new Cart
            {
                UserId = users[0].Id,
                Payed = DateTime.Now,
                Address = users[0].Address,
                Mobile = users[0].Mobile,
                Status = Status.Created,
                Items = new List<CartItem>()
            };

            _context.Carts.Add(cart);
            _context.SaveChanges();

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    CartId = cart.Id,
                    ProductId = products[0].Id,
                    Quantity = 1,
                    Price = products[0].Price
                },
                new CartItem
                {
                    CartId = cart.Id,
                    ProductId = products[1].Id,
                    Quantity = 2,
                    Price = products[1].Price
                }
            };

            _context.CartItems.AddRange(cartItems);
            _context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
