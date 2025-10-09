using Attendance.Application.Interface;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Attendance.Controllers
{
    public class BaseLeaveController : Controller
    {
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuItemService _menuItemService;

        public BaseLeaveController(IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService, IMenuItemService menuItemService)
        {
            _menuService = menuService;
            _userMenuMappingService = userMenuMappingService;
            _menuItemService = menuItemService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            var role = UserUtility.GetRole(HttpContext);

            if (role == "Admin" || role == "SuperAdmin")
            {
                var menuItems = await _menuItemService.GetAll();
                var allMenuMasters = await _menuService.GetAllMenuMasters();

                foreach (var menuItem in menuItems)
                {
                    menuItem.MenuMaster = allMenuMasters.FirstOrDefault(m => m.Menuid == menuItem.Menuid);
                }

                var btclockMenuList = menuItems.Where(x => allMenuMasters.Any(m => m.Menuid == x.Menuid && m.ModuleMasterId == 7)).ToList();
                var topLevelItems = btclockMenuList.Where(m => m.ParentId == 0 || m.ParentId == null).ToList();

                foreach (var item in topLevelItems)
                {
                    item.Children = btclockMenuList.Where(m => m.ParentId == item.MenuItemId).ToList();
                }

                ViewBag.MainMenu = topLevelItems;
            }
            else
            {
                var userId = UserUtility.GetUserId(HttpContext);
                var userMenus = await _userMenuMappingService.GetUserMenuById(Convert.ToInt32(userId));

                // Get MenuItemIds for the user
                var userMenuList = userMenus.Select(x => x.MenuItemId).ToList();
                var menuItems = await _menuItemService.GetAll();
                var allMenuMasters = await _menuService.GetAllMenuMasters();

                // Assign MenuMaster to each MenuItem
                foreach (var menuItem in menuItems)
                {
                    menuItem.MenuMaster = allMenuMasters.FirstOrDefault(m => m.Menuid == menuItem.Menuid);
                }

                // Filter the menus for this user by their allowed `MenuItemId` and `ModuleMasterId = 1`
                var userMenuAccessList = menuItems
                    .Where(x => userMenuList.Contains(x.MenuItemId) && x.MenuMaster?.ModuleMasterId == 7)
                    .ToList();

                // Get top-level menu items for the filtered list (ParentId = 0 or null)
                var topLevelItems = userMenuAccessList.Where(m => m.ParentId == 0 || m.ParentId == null).ToList();

                // Assign children to top-level menu items
                foreach (var item in topLevelItems)
                {
                    item.Children = userMenuAccessList.Where(m => m.ParentId == item.MenuItemId).ToList();
                }

                ViewBag.MainMenu = topLevelItems;
            }

            await next();
        }
    }
}
