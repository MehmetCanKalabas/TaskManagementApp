using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Controllers;
using TaskManagementApp.Core.DTOs;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;
using TaskManagementApp.Infrastructure.Repositories;
using TaskManagementApp.Infrastructure.Services;

namespace TaskManagementApp.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[EnableCors("AllowAll")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly JwtHelper _jwtHelper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, JwtHelper jwtHelper, IUserService userService, ILogger<UserController> logger)
        {
            _jwtHelper = jwtHelper;
            _userRepository = userRepository;
            _userService = userService;
            _logger = logger;
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

            _logger.LogInformation("User Registered.");
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

        [Authorize]
        [HttpGet("secure-endpoint")]
        public IActionResult SecureEndpoint()
        {
            return Ok("Bu endpoint'e sadece yetkili kullanıcılar erişebilir.");
        }
    }

}
