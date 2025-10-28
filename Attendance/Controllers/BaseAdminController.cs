using Attendance.Application.Interface;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

public class BaseAdminController : Controller
{
    private readonly IMenuMasterService _menuService;
    private readonly IUserMenuMappingService _userMenuMappingService;
    private readonly IMenuItemService _menuItemService;

    private const int ModuleMasterId = 5;

    public BaseAdminController(
        IMenuMasterService menuService,
        IUserMenuMappingService userMenuMappingService,
        IMenuItemService menuItemService)
    {
        _menuService = menuService;
        _userMenuMappingService = userMenuMappingService;
        _menuItemService = menuItemService;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
    {
        try
        {
            var role = UserUtility.GetRole(HttpContext);
            var userId = UserUtility.GetUserId(HttpContext);

            HttpContext.Items["CurrentModuleId"] = ModuleMasterId;

            var allMenuItems = (await _menuItemService.GetAllMenuItems())?.ToList() ?? new List<MenuItemDto>();
            var allMenuMasters = (await _menuService.GetAllMenuMasters())?.ToList() ?? new List<MenuMasterDto>();

            foreach (var menuItem in allMenuItems)
            {
                menuItem.MenuMaster = allMenuMasters.FirstOrDefault(m => m.MenuId == menuItem.MenuId);
            }

            List<MenuItemDto> accessibleMenuItems;

            if (role == "3")
            {
                var userMenus = await _userMenuMappingService.GetAccessibleMenusByUserId(Convert.ToInt32(userId));
                var assignedMenuIds = userMenus.Where(x => x.IsActive).Select(x => x.MenuItemId).ToList();

                var defaultMenus = allMenuItems.Where(m => m.MenuMaster?.IsDefault == true).ToList();
                var assignedMenus = allMenuItems.Where(m => assignedMenuIds.Contains(m.MenuItemId)).ToList();

                accessibleMenuItems = defaultMenus.Union(assignedMenus).Distinct().ToList();
            }
            else
            {
                accessibleMenuItems = allMenuItems;
            }
            accessibleMenuItems = accessibleMenuItems
                .Where(m => m.MenuMaster?.ModuleMaster != null &&
                            m.MenuMaster.ModuleMaster.ModuleMasterId == ModuleMasterId)
                .ToList();

            var topLevelItems = accessibleMenuItems
                .Where(m => m.ParentId == null || m.ParentId == 0)
                .ToList();

            foreach (var parent in topLevelItems)
            {
                parent.Children = accessibleMenuItems
                    .Where(c => c.ParentId == parent.MenuItemId)
                    .ToList();
            }

            ViewBag.MainMenu = topLevelItems;

            var menuAccessList = accessibleMenuItems
                .Select(m => m.MenuName?.Trim())
                .Where(name => !string.IsNullOrEmpty(name))
                .Distinct()
                .ToList();

            string cookieKey = $"MenuAccess_Module{ModuleMasterId}";
            string menuAccessJson = JsonSerializer.Serialize(menuAccessList);
            HttpContext.Items[cookieKey] = menuAccessList;

            HttpContext.Items["CurrentModuleId"] = ModuleMasterId;
            HttpContext.Response.Cookies.Append("CurrentModuleId", ModuleMasterId.ToString());
            HttpContext.Response.Cookies.Append(cookieKey, menuAccessJson);

            System.Diagnostics.Debug.WriteLine($"[CurrentModuleId set] {ModuleMasterId}");
        }
        catch (Exception ex)
        {
            ViewBag.MainMenu = new List<MenuItemDto>();
            System.Diagnostics.Debug.WriteLine($"Error in BaseClockInController: {ex.Message}");
        }

        await next();
    }
}
