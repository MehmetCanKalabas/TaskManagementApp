using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;

namespace TaskManagementApp.Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<UserTask>> GetTasksForUserAsync(string userId);

    }
}
