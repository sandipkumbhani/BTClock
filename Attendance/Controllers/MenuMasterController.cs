using Attendance.Application.Interface;
using Attendance.Controllers;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    [Authorize]
    public class MenuMasterController : BaseAdminController
    {
        private readonly ILogger<MenuMasterController> _logger;
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly IMenuMasterService _menuService;
        private readonly IModuleMasterService _moduleMasterService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuItemService _menuItemService;
        private readonly IUserService _userService;
        public MenuMasterController(ILogger<MenuMasterController> logger, IConfiguration configuration, IMenuMasterService menuService, IModuleMasterService moduleMasterService, IUserMenuMappingService userMenuMappingService, IMenuItemService menuItemService,IUserService userService) : base(menuService, userMenuMappingService, menuItemService)
        {
            _logger = logger;
            _configuration = configuration;
            _globalClass = new GlobalClass();
            applicationURL = new ApplicationURL(configuration);
            _menuService = menuService;
            _moduleMasterService = moduleMasterService;
            _userMenuMappingService = userMenuMappingService;
            _menuItemService = menuItemService;
      			_userService = userService;
	    	}
        public async Task<IActionResult> MenuMaster()
        {
            var modules = await _moduleMasterService.GetAllModuleMaster();
            ViewBag.Modules = modules;
            var menuname = await _menuService.GetAllMenuMasters();
            ViewBag.menu = menuname;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MenuMaster(menuMasterDto menuDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingMenu = await _menuService.GetAllMenuMasters();
                    var menuexists = existingMenu.Any(m => m.Menuname.Equals(menuDto.Menuname, StringComparison.OrdinalIgnoreCase));
                    if (menuexists)
                    {
                        ViewBag.errormsg = "Menu Already Exist";
                    }
                    else
                    {
                        var userid = UserUtility.GetUserId(HttpContext);
                        await _menuService.AddMenuMaster(new menuMasterDto
                        {
                            Menuname = menuDto.Menuname,
                            MenuPath = menuDto.MenuPath,
                            Icon = menuDto.Icon,
                            isDefault = menuDto.isDefault,
                            CreatedAt = DateTime.Now,
                            CreatedBy = Convert.ToInt32(userid),
                            ModuleMasterId = menuDto.ModuleMasterId,
                            ParentId = menuDto.ParentId,
                        });
                        ViewBag.appUrl = applicationURL.url;
                        ViewBag.msg = "Menu saved successfully!";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving menu");
                    ViewBag.msg = "An error occurred while saving the menu.";
                }
            }
            else
            {
                ViewBag.errormsg = "Fill The Form.";
            }
            var modules = await _moduleMasterService.GetAllModuleMaster();
            ViewBag.Modules = modules;
            var menuname = await _menuService.GetAllMenuMasters();
            ViewBag.menu = menuname;
            return View(menuDto);
        }

        public async Task<IActionResult> MasterView()
        {
            return View();
        }
        //public async Task<IActionResult> MasterViewDetails()
        //{
        //    var menuList = await _menuService.GetAllMenuMasters();
        //    var users = await _userMenuMappingService.GetAllEmployees();
        //    return Json(new { result = "success", data = menuList, users });
        //}
        public async Task<IActionResult> MasterViewDetails()
        {
            var menuList = await _menuService.GetAllMenuMasters();
            //var employees = await _userMenuMappingService.GetAllUser();
            var user = await _userService.GetAllUser();
			var users = user.Select(e => new
            {
                id = e.UserId,
                name = e.Name
            }).ToList();

            return Json(new { result = "success", data = menuList, users });
        }

        public async Task<IActionResult> UpdateMenu(int id)
        {
            var menu = await _menuService.GetById(id);
            if (menu == null)
            {
                return NotFound();
            }
            var modules = await _moduleMasterService.GetAllModuleMaster();
            ViewBag.Modules = modules;
            var menuname = await _menuService.GetAllMenuMasters();
            ViewBag.menu = menuname;
            return View(menu);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMenu(menuMasterDto menu)
        {
            if (ModelState.IsValid)
            {
                var menuList = await _menuService.GetAllMenuMasters();
                var menuExists = menuList.Any(m => m.Menuname.Equals(menu.Menuname, StringComparison.OrdinalIgnoreCase) && m.Menuid != menu.Menuid);
                if (menuExists)
                {
                    ViewBag.errormsg = "Menu Already Exist";
                    return View(menu);
                }
                else
                {
                    var existingMenu = await _menuService.GetById(menu.Menuid);
                    if (existingMenu == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var currentUserId = UserUtility.GetUserId(HttpContext);
                        var menumodel = new menuMasterDto
                        {
                            Menuname = menu.Menuname,
                            MenuPath = menu.MenuPath,
                            Icon = menu.Icon,
                            isDefault = menu.isDefault,
                            IsActive = existingMenu.IsActive,
                            CreatedAt = existingMenu.CreatedAt,
                            CreatedBy = existingMenu.CreatedBy,
                            UpdatedBy = Convert.ToInt32(currentUserId),
                            UpdatedAt = DateTime.Now,
                            ModuleMasterId = menu.ModuleMasterId,
                            ParentId = menu.ParentId
                        };
                        await _menuService.UpdateMenuMaster(menumodel, menu.Menuid);
                    }
                    TempData["msg"] = "Data updated successfully!";
                    return RedirectToAction("MasterView", "MenuMaster");
                }

            }
            else
            {
                ViewBag.errormsg = "Fill The Form.";
            }
            var menudto = await _menuService.GetById(menu.Menuid);
            var modules = await _moduleMasterService.GetAllModuleMaster();
            ViewBag.Modules = modules;
            var menuname = await _menuService.GetAllMenuMasters();
            ViewBag.menu = menuname;
            return View(menudto);
        }


        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                await _menuService.DeleteMenuMaster(id);
                return RedirectToAction("MasterView", "MenuMaster");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu");
                return Json(new { success = false, message = "An error occurred while deleting the menu." });
            }
        }
    }
}