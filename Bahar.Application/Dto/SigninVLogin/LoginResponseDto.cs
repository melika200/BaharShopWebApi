using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahar.Application.Dto.SigninVLogin
{
    public class LoginResponseDto
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
