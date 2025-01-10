using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Core.DTOs
{
    public class LoginDto
    {
        public string IdentityNumber { get; set; }
        public string Password { get; set; }
    }
}
