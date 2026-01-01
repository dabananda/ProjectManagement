using Microsoft.AspNetCore.Identity;

namespace ProjectManagement.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
