using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models.Entities;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Range(typeof(decimal), "0", "9999999999999999")]
    public decimal PurchasePrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Range(typeof(decimal), "0", "9999999999999999")]
    public decimal SellingPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    [StringLength(30)]
    public string Unit { get; set; } = string.Empty;

    public Category? Category { get; set; }
}
