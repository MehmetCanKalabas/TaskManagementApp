using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IRepository<UserTask> _repository;
        private readonly IValidator<UserTask> _taskValidator;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TasksController> _logger;

        public TasksController(IRepository<UserTask> repository, IValidator<UserTask> taskValidator, ITaskRepository taskRepository, ILogger<TasksController> logger)
        {
            _repository = repository;
            _taskValidator = taskValidator;
            _taskRepository = taskRepository;
            _logger = logger;
        }

        // GET api/tasks
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
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
        public async Task<IActionResult> CreateTask([FromBody] UserTask task)
        {
            if (task == null)
                return BadRequest("Task data is required.");

            var validationResult = await _taskValidator.ValidateAsync(task);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _repository.AddAsync(task);
            _logger.LogInformation($"{task.Id} : User Created.");
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        // PUT api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UserTask task)
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

            await _repository.UpdateAsync(task);
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
