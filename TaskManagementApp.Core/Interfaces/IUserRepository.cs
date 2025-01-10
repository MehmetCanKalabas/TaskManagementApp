using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;

namespace TaskManagementApp.Core.Interfaces
{

    public interface IUserRepository
    {
        Task<User> GetUserByIdentityNumberAsync(string identityNumber);
    }
}
