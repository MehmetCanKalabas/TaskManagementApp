using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;
using TaskManagementApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TaskManagementApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdentityNumberAsync(string identityNumber)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.IdentityNumber == identityNumber);
        }

        public async Task<IEnumerable<UserTask>> GetTasksForUserAsync(string userId)
        {
            return await _context.UserTasks
                .Where(task => task.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.AnyAsync(predicate);
        }

    }
}
