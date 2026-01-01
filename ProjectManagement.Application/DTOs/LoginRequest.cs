using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Application.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
