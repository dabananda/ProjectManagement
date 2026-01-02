using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Project;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Domain.Constants;
using System.Security.Claims;

namespace ProjectManagement.API.Controllers
{
    [Authorize]
    public class ProjectsController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.ProjectManager)]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var result = await _projectService.CreateProjectAsync(request, userId);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectById(Guid projectId)
        {
            var result = await _projectService.GetProjectByIdAsync(projectId);

            if (!result.Succeeded)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (userId == null || role == null)
                return Unauthorized();

            var result = await _projectService.GetAllProjectsAsync(userId, role);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

