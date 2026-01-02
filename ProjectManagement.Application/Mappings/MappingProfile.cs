using AutoMapper;
using ProjectManagement.Application.DTOs.Project;
using ProjectManagement.Application.DTOs.Task;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Project Mappings
            CreateMap<CreateProjectRequest, Project>();
            CreateMap<Project, ProjectResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.ProjectManager != null ? $"{src.ProjectManager.FirstName} {src.ProjectManager.LastName}" : "Unknown"))
                .ForMember(dest => dest.TeamLeadName, opt => opt.MapFrom(src => src.TeamLead != null ? $"{src.TeamLead.FirstName} {src.TeamLead.LastName}" : "Unassigned"));

            // Task Mappings
            CreateMap<CreateTaskRequest, ProjectTask>();
            CreateMap<ProjectTask, TaskResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.AssignedToId, opt => opt.MapFrom(src => src.TeamMemberId))
                .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => src.TeamMember != null ? $"{src.TeamMember.FirstName} {src.TeamMember.LastName}" : "Unassigned"));
        }
    }
}
