using Microsoft.AspNetCore.Identity;

namespace DemoMVC.Models.ViewModels;

public class PermissionMatrixViewModel
{
    public IReadOnlyCollection<IdentityRole> Roles { get; init; } = [];
    public IReadOnlyCollection<PermissionMatrixRowViewModel> Rows { get; init; } = [];
    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> GrantedPermissions { get; init; }
        = new Dictionary<string, IReadOnlyCollection<string>>();
}

public class PermissionMatrixRowViewModel
{
    public string ControllerName { get; init; } = string.Empty;
    public string ActionName { get; init; } = string.Empty;

    public string Key => $"{ControllerName}.{ActionName}";
}
