using Microsoft.AspNetCore.Identity;

namespace FirstWebApiProject.Models.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string? Address { get; set; }
    }
}
