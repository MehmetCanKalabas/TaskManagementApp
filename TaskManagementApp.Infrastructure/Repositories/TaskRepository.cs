using Microsoft.EntityFrameworkCore;
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
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserTask>> GetTasksForUserAsync(string userId)
        {
            return await _context.UserTasks
                .Where(task => task.UserId == userId)
                .ToListAsync();
        }
    }
}
