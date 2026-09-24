using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models
{
    public class BenhVien
    {
        [Key]
        public string ID { get; set; } = default!;
        [Required(ErrorMessage = "Bắt buộc nhập họ tên")]
        public string HoTen { get; set; } = default!;
        [Required(ErrorMessage = "Bắt buộc nhập số điện thoại")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đúng 10 số")]
        public string SDT { get; set; } = default!;
        [Required(ErrorMessage = "Bắt buộc nhập CCCD")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "CCCD phải có đúng 12 số")]
        public string CCCD { get; set; } = default!;
    }
}