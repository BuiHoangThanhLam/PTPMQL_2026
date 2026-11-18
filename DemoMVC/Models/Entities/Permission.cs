using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DemoMVC.Models.Entities;

public class Permission
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string ControllerName { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string ActionName { get; set; } = string.Empty;

    [Required]
    [StringLength(450)]
    public string RoleId { get; set; } = string.Empty;

    public IdentityRole? Role { get; set; }
}
