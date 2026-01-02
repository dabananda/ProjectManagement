using ProjectManagement.Application.DTOs.Project;
using ProjectManagement.Application.Wrappers;

namespace ProjectManagement.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ApiResponse<ProjectResponse>> CreateProjectAsync(CreateProjectRequest request, string managerId);
        Task<ApiResponse<ProjectResponse>> GetProjectByIdAsync(Guid projectId);
        Task<ApiResponse<IEnumerable<ProjectResponse>>> GetAllProjectsAsync(string userId, string role);
    }
}
