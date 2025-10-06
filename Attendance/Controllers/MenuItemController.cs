using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
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
    }
}
