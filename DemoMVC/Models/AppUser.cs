using Microsoft.AspNetCore.Identity;

namespace DemoMVC.Models;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
}
