using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bahar.Domain;

namespace Bahar.Application.InterfaceRepository
{
    public interface IProductRepository
    {
        Task<Product> GetById(long id);
        Task<IEnumerable<Product>> GetAll(Expression<Func<Product, bool>> where = null);
        Task<IEnumerable<Product>> GetAllWithCategory(Expression<Func<Product, bool>> where = null);
        Task Add(Product product);
        Task Update(Product product);
        Task Delete(long id);
        Task<IEnumerable<Product>> GetProductsByCategoryId(long categoryId);
    }
}
