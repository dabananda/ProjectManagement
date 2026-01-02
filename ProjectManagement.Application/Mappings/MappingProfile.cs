using AutoMapper;
using ProjectManagement.Application.DTOs.Project;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateProjectRequest, Project>();

            CreateMap<Project, ProjectResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.ProjectManager != null ? $"{src.ProjectManager.FirstName} {src.ProjectManager.LastName}" : "Unknown"))
                .ForMember(dest => dest.TeamLeadName, opt => opt.MapFrom(src => src.TeamLead != null ? $"{src.TeamLead.FirstName} {src.TeamLead.LastName}" : "Unassigned"));
        }
    }
}
