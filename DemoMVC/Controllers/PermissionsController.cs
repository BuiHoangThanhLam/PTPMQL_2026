using System.Reflection;
using DemoMVC.Data;
using DemoMVC.Models.Entities;
using DemoMVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers;

[Authorize(Roles = "Admin")]
[Route("Permissions")]
public class PermissionsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;

    public PermissionsController(
        ApplicationDbContext context,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    // GET: /Permissions
    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles
            .OrderBy(role => role.Name)
            .AsNoTracking()
            .ToListAsync();

        var controllerTypes = typeof(Program).Assembly
            .GetTypes()
            .Where(type =>
                type is { IsPublic: true, IsAbstract: false } &&
                typeof(Controller).IsAssignableFrom(type))
            .ToList();

        var rows = new List<PermissionMatrixRowViewModel>();

        foreach (var controllerType in controllerTypes)
        {
            var controllerName = controllerType.Name.EndsWith("Controller", StringComparison.Ordinal)
                ? controllerType.Name[..^"Controller".Length]
                : controllerType.Name;

            var actionMethods = controllerType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(method =>
                    method.DeclaringType == controllerType &&
                    !method.IsSpecialName &&
                    !method.IsDefined(typeof(NonActionAttribute), true))
                .DistinctBy(method => method.Name)
                .OrderBy(method => method.Name)
                .ToList();

            foreach (var actionMethod in actionMethods)
            {
                rows.Add(new PermissionMatrixRowViewModel
                {
                    ControllerName = controllerName,
                    ActionName = actionMethod.Name
                });
            }
        }

        var savedPermissions = await _context.Permissions
            .AsNoTracking()
            .ToListAsync();

        var grantedPermissions = savedPermissions
            .GroupBy(permission => permission.RoleId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyCollection<string>)group
                    .Select(permission => $"{permission.ControllerName}.{permission.ActionName}")
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList());

        var model = new PermissionMatrixViewModel
        {
            Roles = roles,
            Rows = rows
                .DistinctBy(row => row.Key)
                .OrderBy(row => row.ControllerName)
                .ThenBy(row => row.ActionName)
                .ToList(),
            GrantedPermissions = grantedPermissions
        };

        return View(model);
    }

    [HttpPost("Update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePermission(
        [FromForm] string roleId,
        [FromForm] string controllerName,
        [FromForm] string actionName,
        [FromForm] bool isGranted)
    {
        if (string.IsNullOrWhiteSpace(roleId) ||
            string.IsNullOrWhiteSpace(controllerName) ||
            string.IsNullOrWhiteSpace(actionName))
        {
            return BadRequest(new { success = false, message = "Thông tin phân quyền không hợp lệ." });
        }

        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is null)
        {
            return BadRequest(new { success = false, message = "Role không tồn tại." });
        }

        var existingPermission = await _context.Permissions
            .FirstOrDefaultAsync(permission =>
                permission.RoleId == roleId &&
                permission.ControllerName == controllerName &&
                permission.ActionName == actionName);

        if (isGranted)
        {
            if (existingPermission is null)
            {
                _context.Permissions.Add(new Permission
                {
                    RoleId = roleId,
                    ControllerName = controllerName.Trim(),
                    ActionName = actionName.Trim()
                });
            }
        }
        else if (existingPermission is not null)
        {
            _context.Permissions.Remove(existingPermission);
        }

        await _context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            message = isGranted
                ? "Đã cấp quyền thành công."
                : "Đã bỏ quyền thành công."
        });
    }
}
