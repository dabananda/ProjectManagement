using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Task;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Domain.Constants;
using System.Security.Claims;

namespace ProjectManagement.API.Controllers
{
    [Authorize]
    public class TasksController : BaseController
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.TeamLeader)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var result = await _taskService.CreateTaskAsync(request, userId);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (userId == null || role == null)
                return Unauthorized();

            var result = await _taskService.GetAllTasksAsync(userId, role);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> GetTaskById(Guid taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
                return NotFound(task);

            return Ok(task);
        }
    }
}
