using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models.Entities;

public class Customer
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [StringLength(256)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Point { get; set; }
}
