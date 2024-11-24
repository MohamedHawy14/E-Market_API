using System.ComponentModel.DataAnnotations;

namespace FirstWebApiProject.DTO.Identity
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        
        public string Password { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [StringLength(50, ErrorMessage = "User name cannot exceed 50 characters.")]
        public string UserName { get; set; }
    }
}
