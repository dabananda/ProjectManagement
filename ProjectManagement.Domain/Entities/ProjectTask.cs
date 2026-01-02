using ProjectManagement.Domain.Constants;
using ProjectManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Domain.Entities
{
    public class ProjectTask : BaseClass
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        public ProjectTaskStatus Status { get; set; }
        public Priority Priority { get; set; }

        public string? SubmissionContent { get; set; }
        public string? SubmissionFileUrl { get; set; }
        public DateTime? SubmittedAt { get; set; }

        public Guid ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }

        public string? TeamMemberId { get; set; }
        [ForeignKey(nameof(TeamMemberId))]
        public virtual ApplicationUser? TeamMember { get; set; }
    }
}
