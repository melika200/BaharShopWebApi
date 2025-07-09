using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bahar.Domain;

namespace Bahar.Application.InterfaceRepository
{
     public interface ICategoryRepository
    {
        Task<Category> GetById(long id);
        Task<IEnumerable<Category>> GetAll(Expression<Func<Category, bool>> where = null);
        Task<List<Product>> GetProductsByCategory(long categoryId);

        Task<bool> CategoryExists(long id);
        Task Add(Category category);
        Task Update(Category category);
        Task Delete(long id);
    }
}
