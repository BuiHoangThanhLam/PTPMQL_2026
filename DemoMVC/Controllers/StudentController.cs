namespace DemoMVC.Controllers
{
    using DemoMVC.Models;
    using Microsoft.AspNetCore.Mvc;

    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Student std)
        {
            var result = "Hello " + std.FullName + " (" + std.StudentCode + ") - " + std.Address;
            ViewBag.Message = result;
            return View();
        }
    }
}