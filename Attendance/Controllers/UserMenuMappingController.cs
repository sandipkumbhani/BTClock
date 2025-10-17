using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    [Authorize]
    public class UserMenuMappingController : BaseAdminController
    {
        private readonly ILogger<UserMenuMappingController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationURL _applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuMasterService _menuService;
        private readonly IUserService _userService;
        private readonly IMenuItemService _menuItemService;

        public UserMenuMappingController(
            ILogger<UserMenuMappingController> logger,
            IConfiguration configuration,
            IUserMenuMappingService userMenuMappingService,
            IMenuMasterService menuService,
            IUserService userService,
            IMenuItemService menuItemService) : base(menuService, userMenuMappingService, menuItemService)
        {
            _logger = logger;
            _configuration = configuration;
            _globalClass = new GlobalClass();
            _applicationURL = new ApplicationURL(configuration);
            _userMenuMappingService = userMenuMappingService;
            _menuService = menuService;
            _userService = userService;
            _menuItemService = menuItemService;
        }

        public async Task<IActionResult> UserMenuMapping()
        {
            var users = await _userService.GetAllUser();
            var menus = await _menuService.GetAllMenuMasters();
            var menuItems = await _menuItemService.GetAllMenuItems();
            var validMenus = menuItems.Where(item => menus.Any(menu => menu.MenuId == item.MenuId)).ToList();

            ViewBag.User = users;
            ViewBag.Menus = validMenus;

            return View(new UserMenuMappingDto());
        }

        [HttpPost]
        public async Task<IActionResult> UserMenuMapping(UserMenuMappingDto userMenuMappingDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.errormsg = "Please select user and menu items.";
                return View(userMenuMappingDto);
            }

            try
            {
                if (userMenuMappingDto.MenuItemIds != null && userMenuMappingDto.MenuItemIds.Any())
                {
                    var result = await _userMenuMappingService.UpdateUserMenuMappingsForUser(
                        userMenuMappingDto.UserId,
                        userMenuMappingDto.MenuItemIds
                    );

                    ViewBag.msg = result;

                    var users = await _userService.GetAllUser();
                    var menus = await _menuService.GetAllMenuMasters();
                    var menuItems = await _menuItemService.GetAllMenuItems();
                    var validMenus = menuItems.Where(item => menus.Any(menu => menu.MenuId == item.MenuId)).ToList();

                    ViewBag.User = users;
                    ViewBag.Menus = validMenus;

                    return View(userMenuMappingDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user menu mappings");
                ViewBag.errormsg = "An error occurred while saving the user menu mappings.";
            }

            return View(userMenuMappingDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserMenus(int userId)
        {
            var userMenus = (await _userMenuMappingService.GetUserMenuMappingsByUserId(userId))
                .Select(m => m.MenuItemId)
                .ToList();

            if (!userMenus.Any())
            {
                var menus = await _menuService.GetAllMenuMasters();
                userMenus = menus.Where(m => m.IsDefault).Select(m => m.MenuId).ToList();
            }

            return Json(new { menuIds = userMenus });
        }
    }
}
