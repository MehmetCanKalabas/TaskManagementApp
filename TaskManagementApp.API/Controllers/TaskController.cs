using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementApp.Core.DTOs;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;
using TaskManagementApp.Infrastructure.Services;

namespace TaskManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IRepository<UserTask> _repository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TasksController> _logger;
        private readonly IUserTaskService _userTaskService;

        public TasksController(IRepository<UserTask> repository, ITaskRepository taskRepository, ILogger<TasksController> logger, IUserTaskService userTaskService)
        {
            _repository = repository;
            _taskRepository = taskRepository;
            _logger = logger;
            _userTaskService = userTaskService;
        }

        // GET api/tasks
        //[Authorize]
        //[AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (String.IsNullOrWhiteSpace(userId))
            {
                return NotFound("User Not Found.");
            }

            var userRole = HttpContext.User.FindFirstValue(ClaimTypes.Role);

            if (userRole == "Admin")
            {
                _logger.LogInformation("Admin Listed All Tasks.");
                var result = await _repository.GetAllAsync();
                return Ok(result);
            }

            _logger.LogInformation($"{userId} : User Listed All Tasks.");
            var tasksForUser = await _taskRepository.GetTasksForUserAsync(userId);
            return Ok(tasksForUser);
        }

        // GET api/tasks/5
        //İsteğe bağlı kullanım
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        // POST api/tasks
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] UserTaskRegisterDto task)
        {
            if (task == null)
                return BadRequest("Task data is required.");

            var validationResult = await _userTaskService.RegisterUserAsync(task);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            _logger.LogInformation($"{task.Title} : User Task Created.");

            return Ok(task);
        }

        // PUT api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UserTaskUpdateDto task)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = HttpContext.User.FindFirstValue(ClaimTypes.Role);

            var existingTask = await _repository.GetByIdAsync(id);
            if (existingTask == null)
                return NotFound();

            // Admin rolü kontrolü
            if (userRole != "Admin" && existingTask.UserId != userId)
            {
                return Unauthorized(); // Admin olmayan kullanıcı sadece kendi görevini güncelleyebilir
            }

            await _userTaskService.UpdateUserAsync(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = HttpContext.User.FindFirstValue(ClaimTypes.Role);

            var existingTask = await _repository.GetByIdAsync(id);
            if (existingTask == null)
                return NotFound();

            // Admin rolü kontrolü
            if (userRole != "Admin" && existingTask.UserId != userId)
            {
                return Unauthorized(); // Admin olmayan kullanıcı sadece kendi görevini silebilir
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }

    }
}
