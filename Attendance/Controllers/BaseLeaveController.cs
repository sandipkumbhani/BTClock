using Attendance.Application.Interface;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance.Controllers
{
    public class BaseLeaveController : Controller
    {
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuItemService _menuItemService;

        public BaseLeaveController(
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
                var userId = UserUtility.GetUserId(HttpContext);
                var roleId = UserUtility.GetRoleId(HttpContext);

                var allMenuItems = (await _menuItemService.GetAllMenuItems())?.ToList() ?? new List<MenuItemDto>();
                var allMenuMasters = (await _menuService.GetAllMenuMasters())?.ToList() ?? new List<MenuMasterDto>();

                foreach (var menuItem in allMenuItems)
                {
                    menuItem.MenuMaster = allMenuMasters.FirstOrDefault(m => m.MenuId == menuItem.MenuId);
                }

                List<MenuItemDto> accessibleMenuItems;
                if (roleId == 3) 
                {
                    var userMenus = await _userMenuMappingService.GetUserMenuMappingsByUserId(userId);
                    var assignedMenuIds = userMenus.Where(x => x.IsActive).Select(x => x.MenuItemId).ToList();

                    var defaultMenus = allMenuItems.Where(m => m.MenuMaster?.IsDefault == true).ToList();
                    var assignedMenus = allMenuItems.Where(m => assignedMenuIds.Contains(m.MenuItemId)).ToList();

                    accessibleMenuItems = defaultMenus.Union(assignedMenus).ToList();
                }
                else 
                {
                    accessibleMenuItems = allMenuItems;
                }

                accessibleMenuItems = accessibleMenuItems
                    .Where(m => m.MenuMaster?.ModuleMaster != null && m.MenuMaster.ModuleMaster.ModuleMasterId == 7)
                    .ToList();

                var topLevelItems = accessibleMenuItems.Where(m => m.ParentId == null || m.ParentId == 0).ToList();
                foreach (var parent in topLevelItems)
                {
                    parent.Children = accessibleMenuItems.Where(c => c.ParentId == parent.MenuItemId).ToList();
                }

                ViewBag.MainMenu = topLevelItems;
            }
            catch (Exception ex)
            {
                ViewBag.MainMenu = new List<MenuItemDto>();
            }

            await next();
        }
    }
}
