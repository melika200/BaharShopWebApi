using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bahar.Domain;

namespace Bahar.Application.InterfaceRepository
{
    public interface IUserRepository
    {
        bool Exists(string username, string email);
        void Add(User user);
        User GetByEmail(string email);
        void SaveChanges();
    }
}
