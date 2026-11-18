using DemoMVC.Data;
using DemoMVC.Models.Entities;
using DemoMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers;

[Route("Product")]
[Route("Products")]
[Route("Product/[action]/{id?}")]
public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Product hoặc /Products
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // GET: trả tbody và phân trang qua Ajax.
    [HttpGet("/Product/Table")]
    [HttpGet("/Products/Table")]
    public async Task<IActionResult> Table(string? search, int page = 1, int pageSize = 10)
    {
        page = Math.Max(page, 1);
        pageSize = pageSize is 10 or 25 or 50 ? pageSize : 10;

        var query = _context.Products
            .AsNoTracking()
            .Include(product => product.Category)
            .Where(product =>
                string.IsNullOrWhiteSpace(search) ||
                product.Sku.Contains(search) ||
                product.Name.Contains(search) ||
                (product.Category != null && product.Category.Name.Contains(search)));

        var totalItems = await query.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));
        page = Math.Min(page, totalPages);

        var items = await query
            .OrderBy(product => product.Name)
            .ThenBy(product => product.Sku)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var model = new ProductTableViewModel
        {
            Items = items,
            SearchTerm = search?.Trim() ?? string.Empty,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };

        return PartialView("_ProductTablePartial", model);
    }

    [HttpGet("/Product/Create")]
    [HttpGet("/Products/Create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await GetCategorySelectListAsync();
        ViewBag.ModalMode = "Create";
        return PartialView("_ProductForm", new Product());
    }

    [HttpPost("/Product/Create")]
    [HttpPost("/Products/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await GetCategorySelectListAsync(product.CategoryId);
            ViewBag.ModalMode = "Create";
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return PartialView("_ProductForm", product);
        }

        var duplicateSku = await _context.Products
            .AsNoTracking()
            .AnyAsync(item => item.Sku == product.Sku);
        if (duplicateSku)
        {
            ModelState.AddModelError(nameof(Product.Sku), "Mã SKU đã tồn tại.");
            ViewBag.Categories = await GetCategorySelectListAsync(product.CategoryId);
            ViewBag.ModalMode = "Create";
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return PartialView("_ProductForm", product);
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return Json(new { success = true, message = "Thêm sản phẩm thành công." });
    }

    [HttpGet("/Product/Edit/{id:int}")]
    [HttpGet("/Products/Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        ViewBag.Categories = await GetCategorySelectListAsync(product.CategoryId);
        ViewBag.ModalMode = "Edit";
        return PartialView("_ProductForm", product);
    }

    [HttpPost("/Product/Edit/{id:int}")]
    [HttpPost("/Products/Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await GetCategorySelectListAsync(product.CategoryId);
            ViewBag.ModalMode = "Edit";
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return PartialView("_ProductForm", product);
        }

        var duplicateSku = await _context.Products
            .AsNoTracking()
            .AnyAsync(item => item.Id != id && item.Sku == product.Sku);
        if (duplicateSku)
        {
            ModelState.AddModelError(nameof(Product.Sku), "Mã SKU đã tồn tại.");
            ViewBag.Categories = await GetCategorySelectListAsync(product.CategoryId);
            ViewBag.ModalMode = "Edit";
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return PartialView("_ProductForm", product);
        }

        var trackedProduct = await _context.Products.FindAsync(id);
        if (trackedProduct is null)
        {
            return NotFound();
        }

        trackedProduct.Sku = product.Sku.Trim();
        trackedProduct.Name = product.Name.Trim();
        trackedProduct.CategoryId = product.CategoryId;
        trackedProduct.PurchasePrice = product.PurchasePrice;
        trackedProduct.SellingPrice = product.SellingPrice;
        trackedProduct.StockQuantity = product.StockQuantity;
        trackedProduct.Unit = product.Unit.Trim();

        await _context.SaveChangesAsync();
        return Json(new { success = true, message = "Cập nhật sản phẩm thành công." });
    }

    [HttpGet("/Product/Details/{id:int}")]
    [HttpGet("/Products/Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(item => item.Category)
            .FirstOrDefaultAsync(item => item.Id == id);

        return product is null
            ? NotFound()
            : PartialView("_ProductForm", product);
    }

    [HttpPost("/Product/Delete/{id:int}")]
    [HttpPost("/Products/Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return Json(new { success = true, message = "Đã xóa sản phẩm." });
    }

    private async Task<List<SelectListItem>> GetCategorySelectListAsync(int selectedCategoryId = 0)
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .Select(category => new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.Name,
                Selected = category.Id == selectedCategoryId
            })
            .ToListAsync();
    }

}
