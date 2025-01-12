using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Core.DTOs;
using TaskManagementApp.Core.Entities;

namespace TaskManagementApp.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsUserExistAsync(string identityNumber);
        Task<FluentValidation.Results.ValidationResult> RegisterUserAsync(UserRegisterDto user);
    }
}
