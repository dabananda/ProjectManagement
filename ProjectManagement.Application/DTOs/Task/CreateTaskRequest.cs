using ProjectManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Application.DTOs.Task
{
    public class CreateTaskRequest
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public Priority Priority { get; set; }

        [Required]
        public string TeamMemberId { get; set; }
    }
}
