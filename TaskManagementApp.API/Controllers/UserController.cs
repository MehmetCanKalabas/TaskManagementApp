using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Core.DTOs;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;
using TaskManagementApp.Infrastructure.Repositories;
using TaskManagementApp.Infrastructure.Services;

namespace TaskManagementApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _repository;
        private readonly IValidator<User> _validator;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly JwtHelper _jwtHelper;

        public UserController(IRepository<User> repository, IValidator<User> validator, IUserRepository userRepository, JwtHelper jwtHelper, IUserService userService)
        {
            _jwtHelper = jwtHelper;
            _repository = repository;
            _validator = validator;
            _userRepository = userRepository;
            _userService = userService;
        }

        // POST api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            if (userDto == null)
                return BadRequest("User data is required.");

            var isUserExist = await _userService.IsUserExistAsync(userDto.IdentityNumber);
            if (isUserExist)
                return BadRequest("A user with the same identity number already exists.");

            var validationResult = await _userService.RegisterUserAsync(userDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return Ok("User registered successfully.");
        }

        // POST api/user/login
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
