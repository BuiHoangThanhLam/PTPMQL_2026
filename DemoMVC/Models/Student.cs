namespace DemoMVC.Models
{
    public class Student
    {
        public string StudentCode { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string? Address { get; set; }
    }
}