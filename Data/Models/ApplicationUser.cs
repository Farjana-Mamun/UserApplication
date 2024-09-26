using Microsoft.AspNetCore.Identity;

namespace UserApplication.Models;

public class ApplicationUser : IdentityUser
{
    public DateTime? LastLogin { get; set; }
    public DateTime RegistrationTime { get; set; }
}
