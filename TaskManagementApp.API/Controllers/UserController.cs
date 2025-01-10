using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Core.DTOs;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;
using TaskManagementApp.Infrastructure.Repositories;

namespace TaskManagementApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _repository;
        private readonly IValidator<User> _validator;
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;

        public UserController(IRepository<User> repository, IValidator<User> validator, IUserRepository userRepository, JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
            _repository = repository;
            _validator = validator;
            _userRepository = userRepository;
        }

        // POST api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null)
                return BadRequest("User data is required.");

            // Kullanıcıyı doğrula
            var validationResult = await _validator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _repository.AddAsync(user);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByIdentityNumberAsync(loginDto.IdentityNumber);

            if (user == null || user.Password != loginDto.Password)
            {
                return Unauthorized("Invalid credentials.");
            }

            // JWT token oluşturuyoruz
            var token = _jwtHelper.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }

}
