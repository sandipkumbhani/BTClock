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

        public MenuItemController(ILogger<MenuMasterController> logger, IConfiguration configuration, GlobalClass globalClass, IMenuItemService menuItemService, IMenuMasterService menuMasterService, IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService) : base(menuService, userMenuMappingService)
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
            var menuItems = await _menuItemService.GetAll();
            var users = await _userMenuMappingService.GetAllUser();
            var parents = await _menuMasterService.GetAllMenuMasters();

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
                menu.UpdatedBy
            }).ToList();

            return Json(new
            {
                result = "success",
                data = menuListWithParents,
                parents = parents,
                users = users
            });
        }

    }
}
