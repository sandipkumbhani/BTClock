using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    [Authorize]
    public class MenuItemController : BaseAdminController
    {
        private readonly ILogger<MenuItemController> _logger;
        private readonly IMenuItemService _menuItemService;
        private readonly IMenuMasterService _menuMasterService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IUserService _userService;
        private readonly ApplicationURL _applicationURL;
        private readonly GlobalClass _globalClass;

        public MenuItemController(
            ILogger<MenuItemController> logger,
            IConfiguration configuration,
            IMenuItemService menuItemService,
            IMenuMasterService menuMasterService,
            IUserMenuMappingService userMenuMappingService,
            IUserService userService,
            IMenuMasterService menuService)
            : base(menuService, userMenuMappingService, menuItemService)
        {
            _logger = logger;
            _menuItemService = menuItemService;
            _menuMasterService = menuMasterService;
            _userMenuMappingService = userMenuMappingService;
            _userService = userService;
            _applicationURL = new ApplicationURL(configuration);
            _globalClass = new GlobalClass();
        }

        public async Task<IActionResult> MenuItem()
        {
            ViewBag.MenuItems = await _menuItemService.GetAllMenuItems();
            ViewBag.Menus = await _menuMasterService.GetAllMenuMasters();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMenuItem(MenuItemDto menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.errormsg = "Fill the form.";
                return View("MenuItem", menuItemDto);
            }

            try
            {
                await _menuItemService.AddMenuItem(menuItemDto);
                ViewBag.msg = "Menu Item added successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding MenuItem");
                ViewBag.errormsg = ex.Message;
            }

            ViewBag.MenuItems = await _menuItemService.GetAllMenuItems();
            ViewBag.Menus = await _menuMasterService.GetAllMenuMasters();
            return View("MenuItem", menuItemDto);
        }

        public IActionResult MenuItemView()
        {
            return View();
        }

        public async Task<IActionResult> MenuItemViewDetails()
        {
            var menuItems = await _menuItemService.GetAllMenuItems();
            var menuMasters = await _menuMasterService.GetAllMenuMasters();
            var users = (await _userService.GetAllUser())
                        .Select(u => new { id = u.UserId, name = u.Name })
                        .ToList();
            var parents = (await _menuItemService.GetAllMenuItems()).Select(u => new { parents = u.ParentId }).ToList();
            var menuItemData = menuItems.Select(menu => new
            {
                menu.MenuItemId,
                menu.MenuName,
                menu.ParentId,
                menu.SortingOrder,
                menu.IsActive,
                menu.CreatedAt,
                menu.CreatedBy,
                menu.UpdatedAt,
                menu.UpdatedBy,
                MenuMaster = menu.MenuMaster != null ? new
                {
                    menu.MenuMaster.MenuId,
                    menu.MenuMaster.MenuName,
                    menu.MenuMaster.ModuleMasterId
                } : null
            }).ToList();

            return Json(new { result = "success", data = menuItemData, users, menuMasters ,parents});
        }

        public async Task<IActionResult> UpdateMenuItem(int id)
        {
            var menuItem = await _menuItemService.GetMenuItemById(id);
            if (menuItem == null) return NotFound();

            ViewBag.Menus = await _menuMasterService.GetAllMenuMasters();
            ViewBag.MenuItems = await _menuItemService.GetAllMenuItems();
            return View(menuItem);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMenuItem(MenuItemDto menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.errormsg = "Fill the form correctly.";
                ViewBag.Menus = await _menuMasterService.GetAllMenuMasters();
                ViewBag.MenuItems = await _menuItemService.GetAllMenuItems();
                return View("EditMenuItem", menuItemDto);
            }

            try
            {
                await _menuItemService.UpdateMenuItem(menuItemDto, menuItemDto.MenuItemId);
                TempData["msg"] = "Menu Item updated successfully!";
                return RedirectToAction("MenuItemView");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating MenuItem");
                ViewBag.errormsg = ex.Message;
            }

            ViewBag.Menus = await _menuMasterService.GetAllMenuMasters();
            ViewBag.MenuItems = await _menuItemService.GetAllMenuItems();
            return View(menuItemDto);
        }

        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            try
            {
                await _menuItemService.DeleteMenuItem(id);
                return RedirectToAction("MenuItemView");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting MenuItem");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
