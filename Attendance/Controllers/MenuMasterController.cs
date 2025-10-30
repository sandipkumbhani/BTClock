using Attendance.Application.Interface;
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
        private readonly IMenuMasterService _menuService;
        private readonly IModuleMasterService _moduleMasterService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuItemService _menuItemService;
        private readonly IUserService _userService;
        private readonly ApplicationURL _applicationURL;
        private readonly GlobalClass _globalClass;

        public MenuMasterController(
            IConfiguration configuration,
            IMenuMasterService menuService,
            IModuleMasterService moduleMasterService,
            IUserMenuMappingService userMenuMappingService,
            IMenuItemService menuItemService,
            IUserService userService)
            : base(menuService, userMenuMappingService, menuItemService)
        {
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
            var user = UserUtility.GetUserId(HttpContext);
            menuMasterDto.CompanyId = Convert.ToInt32(user);


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
                ViewBag.errormsg = "Menu failed!";
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

        public async Task<IActionResult> UpdateMenuMaster(int id)
        {
            var menu = await _menuService.GetById(id);
            if (menu == null) return NotFound();

            ViewBag.Modules = await _moduleMasterService.GetAllModuleMaster();
            ViewBag.menu = await _menuService.GetAllMenuMasters();
            return View(menu);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMenuMaster(MenuMasterDto menuMasterDto)
        {
            var companyId = UserUtility.GetCompanyId(HttpContext);
            var userId = UserUtility.GetUserId(HttpContext);

            menuMasterDto.CompanyId = Convert.ToInt32(companyId);
            menuMasterDto.UpdatedBy = Convert.ToInt32(userId);

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
                ViewBag.errormsg = ex.Message;
            }

            ViewBag.Modules = await _moduleMasterService.GetAllModuleMaster();
            ViewBag.menu = await _menuService.GetAllMenuMasters();
            return View(menuMasterDto);
        }

        public async Task<IActionResult> DeleteMenuMaster(int id)
        {
            try
            {
                await _menuService.DeleteMenuMaster(id);
                return RedirectToAction("MasterView");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
