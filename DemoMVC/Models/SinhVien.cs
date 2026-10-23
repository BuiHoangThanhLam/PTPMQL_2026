using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models
{
    public class SinhVien
    {
        [Key]
        public int ID { get; set; } = default!;
        [Required(ErrorMessage = "Bắt buộc nhập mã sinh viên")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mã sinh viên phải có đúng 10 ký tự")]
        public string MaSV { get; set; } = default!;
        [Required(ErrorMessage = "Bắt buộc nhập họ tên")]
        public string HoTen { get; set; } = default!;
        [Required(ErrorMessage = "Bắt buộc nhập số điện thoại")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đúng 10 số")]
        public string SDT { get; set; } = default!;
        [Required(ErrorMessage = "Bắt buộc nhập địa chỉ")]
        public string DiaChi { get; set; } = default!;
        [Required(ErrorMessage = "Bắt buộc nhập ngày sinh")]
        public DateTime NgaySinh { get; set; } = default!;
        [Required(ErrorMessage = "Bắt buộc chọn giới tính")]
        public string GioiTinh { get; set; } = default!;
        [Display(Name = "Năm nhập học")]
        public int NamNhapHoc { get; set; } = DateTime.Now.Year;

        [Display(Name = "Hệ đào tạo")]
        public string HeDaoTao { get; set; } = "21"; 

        // KHÓA NGOẠI LIÊN KẾT BẢNG KHOA
        [Display(Name = "Khoa / Ngành")]
        public int KhoaID { get; set; }

        [ForeignKey("KhoaID")]
        public Khoa? Khoa { get; set; }
    }
}