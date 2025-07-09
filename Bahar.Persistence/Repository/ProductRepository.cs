using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bahar.Application.InterfaceContext;
using Bahar.Application.InterfaceRepository;
using Bahar.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bahar.Persistence.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDatabaseContext _context;

        public ProductRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public async Task Add(Product product)
        {

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var product = await GetById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAll(Expression<Func<Product, bool>> where = null)
        {
            IQueryable<Product> products = _context.Products;

            if (where != null)
            {
                products = products.Where(where);
            }

            return await products.ToListAsync();
        }
       
        public async Task<IEnumerable<Product>> GetAllWithCategory(Expression<Func<Product, bool>> where = null)
        {
            IQueryable<Product> products = _context.Products.Include(p => p.Category);

            if (where != null)
            {
                products = products.Where(where);
            }

            return await products.ToListAsync();
        }


        public async Task<Product> GetById(long id)
        {
            return await _context.Products
               .Include(p => p.Category)
               .FirstOrDefaultAsync(p => p.Id == id);
        }
       
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(long categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .ToListAsync();
        }


        public async Task Update(Product product)
        {

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

    
    }
}
