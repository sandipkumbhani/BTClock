using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    public class MenuItemController : BaseAdminController
    {
        private readonly ILogger<MenuMasterController> _logger;
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly IMenuItemService _menuItemService;
        private readonly IMenuMasterService _menuMasterService;
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;

        public MenuItemController(ILogger<MenuMasterController> logger, IConfiguration configuration, GlobalClass globalClass, IMenuItemService menuItemService, IMenuMasterService menuMasterService, IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService) : base(menuService, userMenuMappingService, menuItemService)
        {
            _logger = logger;
            _configuration = configuration;
            _globalClass = globalClass;
            applicationURL = new ApplicationURL(configuration);
            _menuItemService = menuItemService;
            _menuMasterService = menuMasterService;
            _menuService = menuService;
            _userMenuMappingService = userMenuMappingService;
		}

        public async Task<IActionResult> MenuItem()
        {
            var menuItems = await _menuItemService.GetAll();
            ViewBag.MenuItems = menuItems;
            var menus = await _menuMasterService.GetAllMenuMasters();
            ViewBag.Menus = menus;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MenuItem(MenuItemDto menuItemDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingMenuItems = await _menuItemService.GetAll();
                    var menuItemExists = existingMenuItems.Any(m => m.MenuName.Equals(menuItemDto.MenuName, StringComparison.OrdinalIgnoreCase));
                    if (menuItemExists)
                    {
                        ViewBag.errormsg = "Menu Item Already Exists";
                    }
                    else
                    {
                        var allMenus = await _menuMasterService.GetAllMenuMasters();
                        var selectedMenu = allMenus.FirstOrDefault(x => x.Menuid == menuItemDto.Menuid);

                        if (selectedMenu == null)
                        {
                            ViewBag.errormsg = "Selected Menu not found.";
                        }
                        else
                        {
                            var userid = UserUtility.GetUserId(HttpContext);
                            await _menuItemService.AddMenuItem(new MenuItemDto
                            {
                                Menuid = menuItemDto.Menuid,
                                MenuName = selectedMenu.Menuname,
                                ParentId = menuItemDto.ParentId,
                                SortingOrder = menuItemDto.SortingOrder,
                                CreatedBy = userid,
                                CreatedAt = DateTime.Now,
                            });

                            ViewBag.succmsg = "Menu Item Added Successfully";
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in MenuItem POST");
                    ViewBag.errormsg = "An error occurred while processing your request.";
                }
            }

            var menuItems = await _menuItemService.GetAll();
            ViewBag.MenuItems = menuItems;
            var menus = await _menuMasterService.GetAllMenuMasters();
            ViewBag.Menus = menus;
            return View(menuItemDto);
        }

        public async Task<IActionResult> MenuItemView()
        {
            return View();
        }
        public async Task<IActionResult> MenuItemViewDetails()
        {
            var menuItems = await _menuItemService.GetAll();  // Get all menu items with MenuMaster
            var parents = await _menuMasterService.GetAllMenuMasters();  // Get all parent menus

            var menuListWithParents = menuItems.Select(menu => new
            {
                menu.MenuItemId,
                menu.MenuName,
                menu.ParentId,
                menu.SortingOrder,
                menu.CreatedAt,
                menu.CreatedBy,
                menu.IsActive,
                menu.UpdatedAt,
                menu.UpdatedBy,
                MenuMaster = menu.MenuMaster != null ? new
                {
                    menu.MenuMaster.Menuid,
                    menu.MenuMaster.Menuname,
                    menu.MenuMaster.ModuleMasterId
                } : null  // Ensure MenuMaster is included
            }).ToList();

            return Json(new
            {
                result = "success",
                data = menuListWithParents,
                parents = parents
            });
        }

        public async Task<IActionResult> UpdateMenuItem(int id)
        {
            var menuItem = await _menuItemService.GetById(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            var menus = await _menuMasterService.GetAllMenuMasters();
            ViewBag.Menus = menus;
            var menuItems = await _menuItemService.GetAll();
            ViewBag.MenuItems = menuItems;
            return View(menuItem);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMenuItem(MenuItemDto menuItem)
        {
            if (ModelState.IsValid)
            {

                var existingMenuItems = await _menuItemService.GetAll();
                var menuItemExists = existingMenuItems.Any(m => m.MenuName.Equals(menuItem.MenuName, StringComparison.OrdinalIgnoreCase) && m.MenuItemId != menuItem.MenuItemId);
                if (menuItemExists)
                {
                    ViewBag.errormsg = "Menu Item Already Exists";
                }
                else
                {
                    var allMenus = await _menuMasterService.GetAllMenuMasters();
                    var selectedMenu = allMenus.FirstOrDefault(x => x.Menuid == menuItem.Menuid);
                    if (selectedMenu == null)
                    {
                        ViewBag.errormsg = "Selected Menu not found.";
                    }
                    else
                    {
                        var userid = UserUtility.GetUserId(HttpContext);
                        var menuitemmodel = new MenuItemDto
                        {
                            MenuItemId = menuItem.MenuItemId,
                            Menuid = menuItem.Menuid,
                            MenuName = selectedMenu.Menuname,
                            ParentId = menuItem.ParentId,
                            SortingOrder = menuItem.SortingOrder,
                            CreatedAt = selectedMenu.CreatedAt,
                            CreatedBy = selectedMenu.CreatedBy,
                            UpdatedBy = userid,
                            UpdatedAt = DateTime.Now,
                            IsActive = menuItem.IsActive
                        };
                        await _menuItemService.UpdateMenuItem(menuitemmodel, menuItem.MenuItemId);
                    }
                    TempData["msg"] = "Data updated successfully!";
                    return RedirectToAction("MenuItemView", "MenuItem");
                }
            }
            else
            {
                ViewBag.errormsg = "Invalid data submitted.";
            }
            var menuitem = await _menuItemService.GetById(menuItem.MenuItemId);
            var menus = await _menuMasterService.GetAllMenuMasters();
            ViewBag.Menus = menus;
            var menuItems = await _menuItemService.GetAll();
            ViewBag.MenuItems = menuItems;
            return View(menuitem);
        }
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            await _menuItemService.DeleteMenuItem(id);
            return RedirectToAction("MenuItemView", "MenuItem");

        }
    }
}