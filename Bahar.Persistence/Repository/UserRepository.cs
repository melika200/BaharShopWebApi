using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bahar.Application.InterfaceContext;
using Bahar.Application.InterfaceRepository;
using Bahar.Domain;

namespace Bahar.Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseContext _context;

        public UserRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public bool Exists(string username, string email)
        {
            return _context.Users.Any(u => u.Username == username || u.Email == email);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public void SaveChanges()
        {
            _context.SaveChangesAsync();
        }
    }
}
