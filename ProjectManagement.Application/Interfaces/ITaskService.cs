using ProjectManagement.Application.DTOs.Task;
using ProjectManagement.Application.Wrappers;

namespace ProjectManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<ApiResponse<TaskResponse>> CreateTaskAsync(CreateTaskRequest request, string teamLeadId);
        Task<ApiResponse<IEnumerable<TaskResponse>>> GetAllTasksAsync(string userId, string role);
        Task<ApiResponse<TaskResponse>> GetTaskByIdAsync(Guid taskId);
    }
}
