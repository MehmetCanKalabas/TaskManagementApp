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

        public UserController(IRepository<User> repository, IValidator<User> validator)
        {
            _repository = repository;
            _validator = validator;
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
                // Hatalıysa, hata mesajları ile birlikte dönebiliriz
                return BadRequest(validationResult.Errors);
            }

            // Eğer doğrulama başarılıysa, kullanıcıyı veritabanına ekle
            await _repository.AddAsync(user);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByIdentityNumberAsync(loginDto.IdentityNumber);

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            if (user.Password != loginDto.Password)
            {
                // Şifre yanlış
                return Unauthorized("Invalid credentials.");
            }

            return Ok("Login successful");
        }
    }

}
