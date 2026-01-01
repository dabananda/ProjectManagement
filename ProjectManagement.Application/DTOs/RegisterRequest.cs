using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Application.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(2, ErrorMessage = "First name should be minimum 2 characters long")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Last name should be minimum 2 characters long")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password should be minimum 6 characters long")]
        public string Password { get; set; }
    }
}
