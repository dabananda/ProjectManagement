using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs.Task;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Application.Wrappers;
using ProjectManagement.Domain.Constants;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;
using ProjectManagement.Infrastructure.Contexts;

namespace ProjectManagement.Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public TaskService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ApiResponse<TaskResponse>> CreateTaskAsync(CreateTaskRequest request, string teamLeadId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId);
            if (project == null)
                return new ApiResponse<TaskResponse>("Project not found");

            if (project.TeamLeadId != teamLeadId)
                return new ApiResponse<TaskResponse>("Unauthorized to create task for this project");

            var member = await _userManager.FindByIdAsync(request.TeamMemberId);
            if (member == null)
                return new ApiResponse<TaskResponse>("Team member not found");

            var task = _mapper.Map<ProjectTask>(request);
            task.Status = ProjectTaskStatus.Pending;
            task.Project = project;
            task.TeamMember = member;

            await _context.ProjectTasks.AddAsync(task);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<TaskResponse>(task);

            return new ApiResponse<TaskResponse>(response);
        }

        public async Task<ApiResponse<IEnumerable<TaskResponse>>> GetAllTasksAsync(string userId, string role)
        {
            IQueryable<ProjectTask> query = _context.ProjectTasks.Include(t => t.Project).Include(t => t.TeamMember).AsNoTracking();

            if (role == null)
                return new ApiResponse<IEnumerable<TaskResponse>>("Unauthorized");
            else if (role == Roles.ProjectManager)
                query = query.Where(t => t.Project.ProjectManagerId == userId.ToString());
            else if (role == Roles.TeamMember)
                query = query.Where(t => t.TeamMemberId == userId.ToString());
            else if (role == Roles.TeamLeader)
                query = query.Where(t => t.Project.TeamLeadId == userId.ToString());

            var tasks = await query.ToListAsync();
            var response = _mapper.Map<IEnumerable<TaskResponse>>(tasks);

            return new ApiResponse<IEnumerable<TaskResponse>>(response);
        }

        public async Task<ApiResponse<TaskResponse>> GetTaskByIdAsync(Guid taskId)
        {
            var task = await _context.ProjectTasks.Include(t => t.Project).Include(t => t.TeamMember).AsNoTracking().FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
                return new ApiResponse<TaskResponse>("Task not found");

            var response = _mapper.Map<TaskResponse>(task);

            return new ApiResponse<TaskResponse>(response);
        }
    }
}
