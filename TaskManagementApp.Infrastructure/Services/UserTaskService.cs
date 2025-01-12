using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Core.DTOs;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;

namespace TaskManagementApp.Infrastructure.Services
{
    public class UserTaskService : IUserTaskService
    {
        private readonly IValidator<UserTask> _validator;
        private readonly IRepository<UserTask> _repository;

        public UserTaskService(IValidator<UserTask> validator, IRepository<UserTask> repository)
        {
            _validator = validator;
            _repository = repository;
        }
        public async Task<FluentValidation.Results.ValidationResult> RegisterUserAsync(UserTaskDto userTaskDto)
        {
            var userTask = new UserTask
            {
                Title = userTaskDto.Title,
                Description = userTaskDto.Description,
                CreatedDate = userTaskDto.CreatedDate,
                UpdatedDate = userTaskDto.UpdatedDate,
                IsCompleted = userTaskDto.IsCompleted,              
            };

            var validationResult = await _validator.ValidateAsync(userTask);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            await _repository.AddAsync(userTask);

            return validationResult;
        }
    }
}
