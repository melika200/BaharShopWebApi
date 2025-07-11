using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bahar.Domain;

namespace Bahar.Application.InterfaceRepository
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
  

}
