using System.ComponentModel.DataAnnotations;

namespace FirstWebApiProject.DTO.Identity
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "User name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9]{4,20}$", ErrorMessage = "User name must be 4-20 characters long and can only contain letters and numbers.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
       
        public string Password { get; set; }
    }
}
