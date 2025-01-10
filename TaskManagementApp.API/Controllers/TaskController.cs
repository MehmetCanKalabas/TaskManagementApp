using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IRepository<UserTask> _taskRepository;
        private readonly IValidator<UserTask> _taskValidator;

        public TasksController(IRepository<UserTask> taskRepository, IValidator<UserTask> taskValidator)
        {
            _taskRepository = taskRepository;
            _taskValidator = taskValidator;
        }

        // GET api/tasks
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return Ok(tasks);
        }

        // GET api/tasks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        // POST api/tasks
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserTask task)
        {
            if (task == null)
                return BadRequest("Task data is required.");

            // Kullanıcı görevini doğrula
            var validationResult = await _taskValidator.ValidateAsync(task);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _taskRepository.AddAsync(task);
            return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
        }

        // PUT api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserTask task)
        {
            if (task == null || task.Id != id)
                return BadRequest();

            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null)
                return NotFound();

            // Kullanıcı görevini doğrula
            var validationResult = await _taskValidator.ValidateAsync(task);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _taskRepository.UpdateAsync(task);
            return NoContent();
        }

        // DELETE api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            await _taskRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
