using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.DTOs.Project;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Application.Wrappers;
using ProjectManagement.Domain.Constants;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;
using ProjectManagement.Infrastructure.Contexts;

namespace ProjectManagement.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly IMapper _mapper;

        public ProjectService(ApplicationDbContext context, UserManager<ApplicationUser> user, IMapper mapper)
        {
            _context = context;
            _user = user;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ProjectResponse>> CreateProjectAsync(CreateProjectRequest request, string managerId)
        {
            var teamLead = await _user.FindByIdAsync(request.TeamLeadId);
            if (teamLead == null)
                return new ApiResponse<ProjectResponse>("Team Lead not found.");

            var isTeamLead = await _user.IsInRoleAsync(teamLead, Roles.TeamLeader);
            if (!isTeamLead)
                return new ApiResponse<ProjectResponse>("The specified user is not a Team Lead.");

            var manager = await _user.FindByIdAsync(managerId);
            if (manager == null)
                return new ApiResponse<ProjectResponse>("Project Manager not found.");

            var project = _mapper.Map<Project>(request);
            project.ProjectManagerId = managerId;
            project.ProjectManager = manager;
            project.TeamLead = teamLead;
            project.Status = ProjectStatus.Assigned;

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<ProjectResponse>(project);

            return new ApiResponse<ProjectResponse>(response, "Project created and assigned successfully.");
        }

        public async Task<ApiResponse<IEnumerable<ProjectResponse>>> GetAllProjectsAsync(string userId, string role)
        {
            IQueryable<Project> query = _context.Projects.Include(p => p.ProjectManager).Include(p => p.TeamLead).AsNoTracking();

            if (role == Roles.ProjectManager)
                query = query.Where(p => p.ProjectManagerId == userId);
            else if (role == Roles.TeamLeader)
                query = query.Where(p => p.TeamLeadId == userId);
            else if (role == Roles.TeamMember)
                query = query.Where(p => p.Tasks.Any(t => t.TeamMemberId == userId));
            else if (role == Roles.User)
                return new ApiResponse<IEnumerable<ProjectResponse>>("Access denied.");

            var projects = await query.ToListAsync();
            var respone = _mapper.Map<IEnumerable<ProjectResponse>>(projects);

            return new ApiResponse<IEnumerable<ProjectResponse>>(respone);
        }

        public async Task<ApiResponse<ProjectResponse>> GetProjectByIdAsync(Guid projectId)
        {
            var project = await _context.Projects.Include(p => p.ProjectManager).Include(p => p.TeamLead).FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return new ApiResponse<ProjectResponse>("Project not found.");

            var response = _mapper.Map<ProjectResponse>(project);

            return new ApiResponse<ProjectResponse>(response);
        }
    }
}
