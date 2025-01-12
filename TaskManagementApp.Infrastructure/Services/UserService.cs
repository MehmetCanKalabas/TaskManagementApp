using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Core.DTOs;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;

namespace TaskManagementApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _validator;
        private readonly IRepository<User> _repository;

        public UserService(IUserRepository userRepository, IValidator<User> validator, IRepository<User> repository)
        {
            _userRepository = userRepository;
            _validator = validator;
            _repository = repository;
        }

        public async Task<bool> IsUserExistAsync(string identityNumber)
        {
            return await _userRepository.ExistsAsync(u => u.IdentityNumber == identityNumber);
        }

        public async Task<FluentValidation.Results.ValidationResult> RegisterUserAsync(UserRegisterDto userDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = userDto.Name,
                IdentityNumber = userDto.IdentityNumber,
                Email = userDto.Email,
                Password = userDto.Password,
                Role = userDto.Role,
            };

            var validationResult = await _validator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            await _repository.AddAsync(user);

            return validationResult;
        }
    }

}
