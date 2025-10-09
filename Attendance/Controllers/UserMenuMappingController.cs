using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

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


		public UserMenuMappingController(ILogger<UserMenuMappingController> logger, IConfiguration configuration, IUserMenuMappingService userMenuMappingService, IMenuMasterService menuService,IUserService userService,IMenuItemService menuItemService) : base(menuService, userMenuMappingService,menuItemService)
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
				var menuItems = await _menuItemService.GetAll();
				var validMenus = menuItems.Where(item => menus.Any(menu => menu.Menuid == item.Menuid)).ToList();
				ViewBag.User = users;
				ViewBag.Menus = validMenus;
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
                        var userMappings = existingMappings.Where(m => m.UserId == userMenuMappingDto.UserId).ToList();
                        var existingMenuIds = userMappings.Select(m => m.MenuItemId ?? 0).ToList();
                        var newMenuIds = userMenuMappingDto.MenuIds;

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
                                    //Id=userMenuMappingDto.MenuItem.Menuid,
                                    UserId = userMenuMappingDto.UserId,
                                    MenuItemId = menuId,

                                    InsertBy = userMenuMappingDto.UserId,
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
                                var existingMapping = userMappings.FirstOrDefault(m => m.MenuItem?.Menuid == menuId);
                                if (existingMapping != null)
                                {
                                    existingMapping.UpdateBy = userMenuMappingDto.UserId;
                                    existingMapping.UpdateDate = DateTime.Now;
                                    await _userMenuMappingService.UpdateMenuMapping(existingMapping, existingMapping.Id);
                                    updateMade = true;
                                }
                            }
                        }

                        if (toDelete.Any())
                        {
                            foreach (var mapping in userMappings.Where(m => toDelete.Contains(m.MenuItemId ?? 0)))
                            {
                                await _userMenuMappingService.DeleteUserMenuMapping(mapping.Id);
                                deleteMade = true;
                            }
                        }

                        if (addMade || deleteMade)
                        {
                            ViewBag.msg = "User menu mapping updated successfully!";
                        }
                        else
                        {
                            ViewBag.errormsg = "No changes were made.";
                        }
                        //var users = await _userMenuMappingService.GetAllUser();
                        var users = await _userService.GetAllUser();
						//var userlist = users.Where(x => x.RoleId != 1).ToList();
						var menus = await _menuService.GetAllMenuMasters();
						var menuItems = await _menuItemService.GetAll();
						var validMenus = menuItems.Where(item => menus.Any(menu => menu.Menuid == item.Menuid)).ToList();
						ViewBag.User = users;
						ViewBag.Menus = validMenus;
						var model = new UserMenuMappingDto();
						return View(userMenuMappingDto);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating user menu mappings");
                    ViewBag.errormsg = "An error occurred while saving the user menu mappings.";
                }
            }
            return View(userMenuMappingDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserMenus(int userId)
        {
            var allMappings = await _userMenuMappingService.GetAll();
            var userMenus = allMappings
                .Where(m => m.UserId == Convert.ToInt32(userId))
                .Select(m => m.MenuItemId ?? 0)
                .ToList();
            if (!userMenus.Any())
            {
                var menus = await _menuService.GetAllMenuMasters();
				userMenus = menus.Where(m => m.isDefault).Select(m => m.Menuid).ToList();
            }

            return Json(new { menuIds = userMenus });
        }
    }
}
