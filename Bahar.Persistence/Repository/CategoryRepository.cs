using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bahar.Application.InterfaceContext;
using Bahar.Application.InterfaceRepository;
using Bahar.Domain;
using Bahar.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bahar.Persistence.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDatabaseContext _context;
        public CategoryRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public async Task Add(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CategoryExists(long id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id); 
        }

        public async Task Delete(long id)
        {
            var category = await GetById(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetAll(Expression<Func<Category, bool>> where = null)
        {
            if (where != null)
            {
                return await _context.Categories.Where(where).ToListAsync();
            }
            else
            {
                return await _context.Categories.ToListAsync();
            }
        }

        public async Task<Category> GetById(long id)
        {
         
            return await _context.Categories.Include(a => a.Products).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Product>> GetProductsByCategory(long categoryId)
        {
          
            return await _context.Categories
                                 .Where(c => c.Id == categoryId)
                                 .SelectMany(c => c.Products)
                                 .ToListAsync();
        }


        public async Task Update(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

      
    }
}
