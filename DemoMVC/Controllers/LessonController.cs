namespace DemoMVC.Controllers
{

    using Microsoft.AspNetCore.Mvc;

    public class LessonController : Controller
    {
        public IActionResult Lesson1()
        {
            return View();
        }
    }
}