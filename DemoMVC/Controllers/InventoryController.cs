using Microsoft.AspNetCore.Mvc;

public class InventoryController : Controller
{
    // HÀNH ĐỘNG 1: Lưu dữ liệu và Chuyển hướng
    [HttpPost]
    public IActionResult SaveDevice()
    {
        // ... (Logic lưu vào cơ sở dữ liệu ở đây) ...

        // Sử dụng TempData vì ta chuẩn bị dùng RedirectToAction. 
        // Dữ liệu này sẽ sống sót nhảy sang được Request tiếp theo.
        TempData["AlertMessage"] = "Đã lưu thiết bị thành công!";
        
        return RedirectToAction("Index"); 
    }

    // HÀNH ĐỘNG 2: Hiển thị trang Danh sách
    [HttpGet]
    public IActionResult Index()
    {
        // Sử dụng ViewBag (dynamic): Gọn nhẹ, gán trực tiếp thuộc tính
        ViewBag.PageTitle = "Danh sách kho hàng";

        // Sử dụng ViewData (Dictionary): Truyền số nguyên, phải lưu qua Key
        ViewData["TotalItems"] = 150;

        // Trả về file Index.cshtml
        return View();
    }
}