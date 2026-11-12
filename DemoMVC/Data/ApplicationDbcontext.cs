using DemoMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Student> Students { get; set; } = default!;
        public DbSet<Employee> Employees { get; set; } = default!;
        public DbSet<BenhVien> BenhViens { get; set; } = default!;
        public object BenhVien { get; internal set; }
        public DbSet<SinhVien> SinhViens { get; set; } = default!;
        public DbSet<Khoa> Khoa { get; set; } = default!;
    }
}