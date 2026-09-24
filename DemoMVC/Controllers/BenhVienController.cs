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
    public class BenhVienController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BenhVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BenhVien
        public async Task<IActionResult> Index()
        {
            return View(await _context.BenhViens.ToListAsync());
        }

        // GET: BenhVien/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benhVien = await _context.BenhViens
                .FirstOrDefaultAsync(m => m.ID == id);
            if (benhVien == null)
            {
                return NotFound();
            }

            return View(benhVien);
        }

        // GET: BenhVien/Create
        [HttpGet]
        public IActionResult Create()
        {
        // Mã mặc định nếu Database chưa có ai
            string newId = "BV-00001"; 

            // Lấy ra bệnh nhân có ID lớn nhất (sắp xếp giảm dần và lấy người đầu tiên)
            var lastRecord = _context.BenhViens
                                    .OrderByDescending(b => b.ID)
                                    .FirstOrDefault();

            // Nếu đã có dữ liệu trong Database
            if (lastRecord != null && lastRecord.ID.StartsWith("BV-"))
            {
                // Cắt bỏ 3 ký tự đầu ("BV-"), chỉ lấy phần số đằng sau
                string numberPart = lastRecord.ID.Substring(3); 
                
                // Ép sang kiểu số nguyên (int)
                if (int.TryParse(numberPart, out int lastNumber))
                {
                    // Cộng thêm 1 và dùng .ToString("D5") để ép format có 5 chữ số (VD: 2 -> 00002)
                    newId = "BV-" + (lastNumber + 1).ToString("D5");
                }
            }

            // Gán mã vừa tính được vào Model và gửi sang View
            var model = new BenhVien 
            { 
                ID = newId 
            };
        return View(model);
        }

        // POST: BenhVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,HoTen,SDT,CCCD")] BenhVien benhVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(benhVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(benhVien);
        }

        // GET: BenhVien/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benhVien = await _context.BenhViens.FindAsync(id);
            if (benhVien == null)
            {
                return NotFound();
            }
            return View(benhVien);
        }

        // POST: BenhVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,HoTen,SDT,CCCD")] BenhVien benhVien)
        {
            if (id != benhVien.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(benhVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BenhVienExists(benhVien.ID))
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
            return View(benhVien);
        }

        // GET: BenhVien/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benhVien = await _context.BenhViens
                .FirstOrDefaultAsync(m => m.ID == id);
            if (benhVien == null)
            {
                return NotFound();
            }

            return View(benhVien);
        }

        // POST: BenhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var benhVien = await _context.BenhViens.FindAsync(id);
            if (benhVien != null)
            {
                _context.BenhViens.Remove(benhVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BenhVienExists(string id)
        {
            return _context.BenhViens.Any(e => e.ID == id);
        }
    }
}
