using ProjectManagement.Domain.Constants;
using ProjectManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Domain.Entities
{
    public class Project : BaseClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ProjectStatus Status { get; set; }
        public Priority Priority { get; set; }

        public string ProjectManagerId { get; set; }
        [ForeignKey(nameof(ProjectManagerId))]
        public virtual ApplicationUser ProjectManager { get; set; }

        public string TeamLeadId { get; set; }
        [ForeignKey(nameof(TeamLeadId))]
        public virtual ApplicationUser TeamLead { get; set; }

        public virtual ICollection<ProjectTask> Tasks { get; set; }
        public virtual ICollection<ProjectFile> ProjectFiles { get; set; }
    }
}
