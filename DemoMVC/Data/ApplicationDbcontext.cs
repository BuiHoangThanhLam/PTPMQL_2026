using DemoMVC.Models;
using DemoMVC.Models.Entities;
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
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Supplier> Suppliers { get; set; } = default!;
        public DbSet<Customer> Customers { get; set; } = default!;
        public DbSet<Permission> Permissions { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ danh mục - sản phẩm và chống xóa danh mục đang được sử dụng.
            modelBuilder.Entity<Product>()
                .HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasIndex(category => category.Name)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(product => product.Sku)
                .IsUnique();

            modelBuilder.Entity<Supplier>()
                .HasIndex(supplier => supplier.Phone);

            modelBuilder.Entity<Customer>()
                .HasIndex(customer => customer.Phone)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(customer => customer.Email)
                .IsUnique();

            // Quyền động tham chiếu đến role Identity chuẩn.
            modelBuilder.Entity<Permission>()
                .HasOne(permission => permission.Role)
                .WithMany()
                .HasForeignKey(permission => permission.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Permission>()
                .HasIndex(permission => new
                {
                    permission.ControllerName,
                    permission.ActionName,
                    permission.RoleId
                })
                .IsUnique();
        }
    }
}