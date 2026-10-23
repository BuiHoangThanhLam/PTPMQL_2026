using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Khoa
    {
        [Key]
        public int KhoaID { get; set; }

        [Required, Display(Name = "Tên Khoa")]
        public string TenKhoa { get; set; } = default!;

        [Required, Display(Name = "Mã Ngành")]
        public string MaNganh { get; set; } = default!; 

        // Liên kết 1 Khoa có nhiều Sinh Viên
        public ICollection<SinhVien>? SinhViens { get; set; }
    }
}