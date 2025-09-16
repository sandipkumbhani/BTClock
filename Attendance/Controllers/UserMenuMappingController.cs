using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    public class UserMenuMappingController : BaseAdminController
    {
        private readonly ILogger<UserMenuMappingController> _logger;
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly IMenuMasterService _menuService;


        public UserMenuMappingController(ILogger<UserMenuMappingController> logger, IConfiguration configuration, IUserMenuMappingService userMenuMappingService, IMenuMasterService menuService) : base(menuService, userMenuMappingService)
        {
            _logger = logger;
            _configuration = configuration;
            _globalClass = new GlobalClass();
            applicationURL = new ApplicationURL(configuration);
            _userMenuMappingService = userMenuMappingService;
            _menuService = menuService;
        }

        public async Task<IActionResult> UserMenuMapping()
        {
            var employees = await _userMenuMappingService.GetAllEmployees();
            //var userlist = users.Where(x => x.DesignationId != 1).ToList();
            var menus = await _menuService.GetAllMenuMasters();
            ViewBag.employees = employees;
            ViewBag.Menus = menus;
            var model = new UserMenuMappingDto();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UserMenuMapping(UserMenuMappingDto userMenuMappingDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (userMenuMappingDto.MenuIds != null && userMenuMappingDto.MenuIds.Any())
                    {
                        var existingMappings = await _userMenuMappingService.GetAll();
                        var userMappings = existingMappings.Where(m => m.EmployeeId == userMenuMappingDto.EmployeeId).ToList();
                        var existingMenuIds = userMappings.Select(m => m.MenuMasterMenuid.GetValueOrDefault()).ToList();

                        var newMenuIds = userMenuMappingDto.MenuIds ?? new List<int>();
                        var toAdd = newMenuIds.Except(existingMenuIds).ToList();
                        var toDelete = existingMenuIds.Except(newMenuIds).ToList();
                        var toUpdate = existingMenuIds.Intersect(newMenuIds).ToList();
                        bool addMade = false, updateMade = false, deleteMade = false;
                        if (toAdd.Any())
                        {
                            foreach (var menuId in toAdd)
                            {
                                var newMapping = new UserMenuMappingDto
                                {
                                    EmployeeId = userMenuMappingDto.EmployeeId,
                                    MenuMasterMenuid = menuId,
                                    InsertBy = userMenuMappingDto.EmployeeId,
                                    InsertDate = DateTime.Now
                                };
                                await _userMenuMappingService.AddUserMenuMapping(newMapping);
                                addMade = true;
                            }
                        }
                        if (toUpdate.Any())
                        {
                            foreach (var menuId in toUpdate)
                            {
                                var existingMapping = userMappings.FirstOrDefault(m => m.MenuMasterMenuid == menuId);
                                if (existingMapping != null)
                                {
                                    existingMapping.UpdateBy = userMenuMappingDto.EmployeeId;
                                    existingMapping.UpdateDate = DateTime.Now;
                                    await _userMenuMappingService.UpdateMenuMapping(existingMapping, existingMapping.Id);
                                    updateMade = true;
                                }
                            }
                        }
                        if (toDelete.Any())
                        {
                            foreach (var mapping in userMappings.Where(m => toDelete.Contains(m.MenuMasterMenuid ?? 0)))
                            {
                                await _userMenuMappingService.DeleteUserMenuMapping(mapping.Id);
                                deleteMade = true;
                            }
                        }
                        if (addMade || deleteMade)
                        {
                            ViewBag.msg = "UserMenu Save successfully!";
                        }
                        else
                        {
                            ViewBag.errormsg = "No changes were made.";
                        }
                        var employees = await _userMenuMappingService.GetAllEmployees();
                        //var userlist = users.Where(x => x.RoleId != 1).ToList();
                        var menus = await _menuService.GetAllMenuMasters();
                        ViewBag.employees = employees;
                        ViewBag.Menus = menus;
                        return View(userMenuMappingDto);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating menu mapping");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeMenus(int employeeId)
        {
            var allMappings = await _userMenuMappingService.GetAll();
            var employeeMenus = allMappings
                .Where(m => m.EmployeeId == Convert.ToInt32(employeeId))
                .Select(m => m.MenuMasterMenuid ?? 0)
                .ToList();
            if (!employeeMenus.Any())
            {
                var menus = await _menuService.GetAllMenuMasters();
                employeeMenus = menus.Where(m => m.isDefault).Select(m => m.Menuid).ToList();
            }

            return Json(new { menuIds = employeeMenus });
        }
    }
}
