using ProjectManagement.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Domain.Entities
{
    public class ProjectFile : BaseClass
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }

        public Guid ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }
    }
}
