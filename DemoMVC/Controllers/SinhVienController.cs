using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoMVC.Data;
using DemoMVC.Models;

namespace DemoMVC.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SinhVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SinhVien
        public async Task<IActionResult> Index()
        {
            // Lệnh Include giúp lôi thêm dữ liệu từ bảng Khoa sang
            var danhSach = _context.SinhViens.Include(s => s.Khoa);
            return View(await danhSach.ToListAsync());
        }

        // GET: SinhVien/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var sinhVien = await _context.SinhViens
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // GET: SinhVien/Create
        [HttpGet]
        public IActionResult Create()
        {
            // Lấy danh sách Khoa từ Database đổ vào Dropdown
            ViewBag.KhoaID = new SelectList(_context.Set<Khoa>(), "KhoaID", "TenKhoa");
            
            return View();
        }

        // POST: SinhVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MaSV,HoTen,SDT,DiaChi,NgaySinh,GioiTinh,NamNhapHoc,HeDaoTao,KhoaID")] SinhVien sinhVien)
        {
            // Xóa xác thực các trường tự sinh để không bị lỗi
            ModelState.Remove("ID");
            ModelState.Remove("MaSV");
            ModelState.Remove("Khoa");

            if (ModelState.IsValid)
            {
                // Logic sinh mã ngầm (chỉ có số, 5 chữ số)
                int newId = 1;
                var lastRecord = _context.SinhViens
                                        .OrderByDescending(s => s.ID)
                                        .FirstOrDefault();
                if (lastRecord != null)
                {
                    newId = lastRecord.ID + 1;
                }
                // Gán mã vừa sinh vào Model
                sinhVien.ID = newId;

                // Lấy thông tin Khoa để lấy Mã Ngành (VD: "05")
                var khoa = await _context.Set<Khoa>().FindAsync(sinhVien.KhoaID);
                string maNganh = khoa?.MaNganh ?? "00";

                // Cắt lấy 2 số cuối của năm (Ví dụ: 2028 -> "28")
                string nam = sinhVien.NamNhapHoc.ToString().Substring(2, 2); 
                string he = sinhVien.HeDaoTao; // "21"
                
                // Tiền tố: 28 + 21 + 05 = 282105
                string prefix = $"{nam}{he}{maNganh}";

                // Tìm sinh viên cuối cùng có chung tiền tố này
                var lastSV = _context.SinhViens
                                    .Where(s => s.MaSV.StartsWith(prefix))
                                    .OrderByDescending(s => s.MaSV)
                                    .FirstOrDefault();

                string stt = "0001"; // Nếu chưa có ai, bắt đầu là 0001
                
                if (lastSV != null)
                {
                    // Cắt 4 số cuối của mã cũ (VD: 2821050199 -> lấy 0199)
                    string lastSttStr = lastSV.MaSV.Substring(prefix.Length);
                    
                    // Cộng thêm 1 và format thành 4 số (-> 0200)
                    if (int.TryParse(lastSttStr, out int lastStt))
                    {
                        stt = (lastStt + 1).ToString("D4"); 
                    }
                }
                
                // Ghép lại thành mã hoàn chỉnh: 282105 + 0200 = 2821050200
                sinhVien.MaSV = prefix + stt;

                // 3. Lưu vào Database
                _context.Add(sinhVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Nếu có lỗi, phải nạp lại danh sách Khoa trước khi trả về View
            ViewBag.KhoaID = new SelectList(_context.Set<Khoa>(), "KhoaID", "TenKhoa", sinhVien.KhoaID);
            return View(sinhVien);
        }

        // GET: SinhVien/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            return View(sinhVien);
        }

        // POST: SinhVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MaSV,HoTen,SDT,DiaChi,NgaySinh,GioiTinh,NamNhapHoc,HeDaoTao,KhoaID")] SinhVien sinhVien)
        {
            if (id != sinhVien.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sinhVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SinhVienExists(sinhVien.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sinhVien);
        }

        // GET: SinhVien/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var sinhVien = await _context.SinhViens
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // POST: SinhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien != null)
            {
                _context.SinhViens.Remove(sinhVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SinhVienExists(int id)
        {
            return _context.SinhViens.Any(e => e.ID == id);
        }
    }
}
