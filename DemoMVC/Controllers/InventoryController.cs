using Microsoft.AspNetCore.Mvc;

public class InventoryController : Controller
{
    // CHIỀU 1: Controller gửi giao diện mặc định và lời nhắn sang View
    [HttpGet]
    public IActionResult Search()
    {
        ViewBag.Title = "Tìm kiếm thiết bị";
        ViewBag.Message = "Nhập từ khóa để bắt đầu tìm kiếm.";
        return View();
    }

    // CHIỀU 2: View gửi từ khóa lên, Controller nhận lấy và phản hồi lại
    [HttpPost]
    public IActionResult Search(string keyword)
    {
        // Tham số 'keyword' nhận trực tiếp giá trị từ thẻ <input name="keyword"> ở View
        if (string.IsNullOrEmpty(keyword))
        {
            ViewBag.Message = "Bạn chưa nhập gì cả!";
        }
        else
        {
            // Trả lại kết quả và chính từ khóa đó qua ViewBag
            ViewBag.Message = $"Bạn vừa tìm kiếm từ khóa: {keyword}";
            ViewBag.OldKeyword = keyword; 
        }
        
        return View();
    }
}