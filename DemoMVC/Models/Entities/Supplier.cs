using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models.Entities;

public class Supplier
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Address { get; set; }
}
