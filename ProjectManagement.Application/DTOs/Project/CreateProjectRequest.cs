using ProjectManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Application.DTOs.Project
{
    public class CreateProjectRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Requirements { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public Priority Priority { get; set; }

        [Required]
        public string TeamLeadId { get; set; }
    }
}
