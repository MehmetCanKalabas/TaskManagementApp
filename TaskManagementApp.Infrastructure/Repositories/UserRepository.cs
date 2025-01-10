using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;
using TaskManagementApp.Infrastructure.Data;

namespace TaskManagementApp.Infrastructure.Repositories
{
    public class UserRepository  /*IUserRepository*/
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<User> GetUserByIdentityNumberAsync(string identityNumber)
        //{
        //    // Kullanıcıyı TC Kimlik Numarasına göre arıyoruz
        //    return await _context.Users
        //        .FirstOrDefaultAsync(u => u.IdentityNumber == identityNumber);
        //}

    }
}
