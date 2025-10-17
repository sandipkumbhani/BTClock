using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    [Authorize]
    public class MenuMasterController : BaseAdminController
    {
        private readonly ILogger<MenuMasterController> _logger;
        private readonly IMenuMasterService _menuService;
        private readonly IModuleMasterService _moduleMasterService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuItemService _menuItemService;
        private readonly IUserService _userService;
        private readonly ApplicationURL _applicationURL;
        private readonly GlobalClass _globalClass;

        public MenuMasterController(
            ILogger<MenuMasterController> logger,
            IConfiguration configuration,
            IMenuMasterService menuService,
            IModuleMasterService moduleMasterService,
            IUserMenuMappingService userMenuMappingService,
            IMenuItemService menuItemService,
            IUserService userService)
            : base(menuService, userMenuMappingService, menuItemService)
        {
            _logger = logger;
            _menuService = menuService;
            _moduleMasterService = moduleMasterService;
            _userMenuMappingService = userMenuMappingService;
            _menuItemService = menuItemService;
            _userService = userService;
            _applicationURL = new ApplicationURL(configuration);
            _globalClass = new GlobalClass();
        }

        public async Task<IActionResult> MenuMaster()
        {
            ViewBag.Modules = await _moduleMasterService.GetAllModuleMaster();
            ViewBag.menu = await _menuService.GetAllMenuMasters();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMenuMaster(MenuMasterDto menuMasterDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.errormsg = "Fill the form.";
                return View(menuMasterDto);
            }

            try
            {
                await _menuService.AddMenuMaster(menuMasterDto);
                ViewBag.msg = "Menu saved successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving menu");
                ViewBag.errormsg = ex.Message;
            }

            ViewBag.Modules = await _moduleMasterService.GetAllModuleMaster();
            ViewBag.menu = await _menuService.GetAllMenuMasters();
            return View("MenuMaster", menuMasterDto);
        }

        public IActionResult MasterView()
        {
            return View();
        }

        public async Task<IActionResult> MasterViewDetails()
        {
            var menuList = await _menuService.GetAllMenuMasters();
            var users = (await _userService.GetAllUser())
                        .Select(u => new { id = u.UserId, name = u.Name })
                        .ToList();

            return Json(new { result = "success", data = menuList, users });
        }

        public async Task<IActionResult> UpdateMenu(int id)
        {
            var menu = await _menuService.GetById(id);
            if (menu == null) return NotFound();

            ViewBag.Modules = await _moduleMasterService.GetAllModuleMaster();
            ViewBag.menu = await _menuService.GetAllMenuMasters();
            return View(menu);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMenu(MenuMasterDto menuMasterDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.errormsg = "Fill the form.";
                return View(menuMasterDto);
            }

            try
            {
                await _menuService.UpdateMenuMaster(menuMasterDto, menuMasterDto.MenuId);
                TempData["msg"] = "Data updated successfully!";
                return RedirectToAction("MasterView");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu");
                ViewBag.errormsg = ex.Message;
            }

            ViewBag.Modules = await _moduleMasterService.GetAllModuleMaster();
            ViewBag.menu = await _menuService.GetAllMenuMasters();
            return View(menuMasterDto);
        }

        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                await _menuService.DeleteMenuMaster(id);
                return RedirectToAction("MasterView");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
