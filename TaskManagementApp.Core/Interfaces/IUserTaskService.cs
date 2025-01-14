using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Core.DTOs;

namespace TaskManagementApp.Core.Interfaces
{
    public interface IUserTaskService
    {
        Task<FluentValidation.Results.ValidationResult> RegisterUserAsync(UserTaskRegisterDto user);
        Task<FluentValidation.Results.ValidationResult> UpdateUserAsync(UserTaskUpdateDto userTaskUpdateDto);
    }
}
