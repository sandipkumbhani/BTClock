using Attendance.Application.Interface;
using Attendance.Application.service;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Attendance.Controllers
{
	public class UserController : BaseAdminController
	{
		private readonly IConfiguration _configuration;
		private ApplicationURL applicationURL;
		private readonly GlobalClass _globalClass;
		private readonly IUserService _userService;
		private readonly IMenuMasterService _menuService;
		private readonly IUserMenuMappingService _userMenuMappingService;
		private readonly IRoleService _roleService;
		private readonly IMenuItemService _menuItemService;


		public UserController(IConfiguration configuration, GlobalClass globalClass, IUserService userService, IUserMenuMappingService userMenuMappingService, IMenuMasterService menuService, IRoleService roleService, IMenuItemService menuItemService) : base(menuService, userMenuMappingService, menuItemService)
		{
			_configuration = configuration;
			applicationURL = new ApplicationURL(_configuration);
			_globalClass = globalClass;
			_userService = userService;
			_userMenuMappingService = userMenuMappingService;
			_menuService = menuService;
			_roleService = roleService;
			_menuItemService = menuItemService;
		}
		public IActionResult User()
		{
			if (_globalClass.Token != null)
			{
				var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_globalClass.Token);
				var claims = UserUtility.addClaimstoUser(HttpContext, jwt.Claims);
				string currentPage = "Add User";
				var canAccess = UserUtility.CanAccessMenu(HttpContext, currentPage);
				if (canAccess)
				{
					var User = claims.Claims.FirstOrDefault(x => x.Type == "UserId");
					int UserId = User != null ? (!string.IsNullOrEmpty(User.Value) ? Convert.ToInt32(User.Value) : 0) : 0;
					ViewBag.UserId = UserId;
					ViewBag.appUrl = applicationURL.url;
					var roles = _roleService.GetAll().Result;
					ViewBag.Roles = roles;
					var users = _userService.GetAllUser().Result;
					ViewBag.Users = users;
					return View();
				}
				else
				{
					return RedirectToAction("AccessDenied", "Home");
				}
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> User(UserDto userDto)
		{
			ViewBag.appUrl = applicationURL.url;
			var userId = UserUtility.GetUserId(HttpContext);

			if (ModelState.IsValid)
			{
				try
				{
					var existingUser = await _userService.GetAllUser();
					var userexists = existingUser.Any(u => u.Email == userDto.Email && u.IsActive == true);
					if (userexists)
					{
						ViewData["errMessage"] = "User Already Exist";
					}
					else
					{
						var user = new UserDto
						{
							UserId = userDto.UserId,
							Name = userDto.Name,
							Email = userDto.Email,
							Password = userDto.Password,
							RoleId = userDto.RoleId,
							PerentId = userDto.PerentId,
							IsActive = true,
							CreatedAt = DateTime.Now,
							CreatedBy = Convert.ToInt16(userId),
						};
						var result = await _userService.AddUser(user);
						if (result != null)
						{
							var getuser = await _userService.GetAllUser();
							var newuser = getuser.FirstOrDefault(u => u.Email == userDto.Email);
							var newuserId = newuser != null ? newuser.UserId : 0;
							var userMenus = await _userMenuMappingService.GetUserMenuById(newuserId);
							if (userMenus.Any())
							{
								foreach (var menuitem in userMenus)
								{
									var mappingDto = new UserMenuMappingDto
									{
										UserId = newuserId,
										MenuItemId = menuitem.MenuItemId,
										InsertBy = userId,
										InsertDate = DateTime.Now
									};
									await _userMenuMappingService.AddUserMenuMapping(mappingDto);
								}
							}
							else
							{
								var menuMasters = await _menuService.GetAllMenuMasters();
								var menuItems = await _menuItemService.GetAll();
							
								var defaultMenuIds = menuMasters
									.Where(mm => mm.isDefault)
									.Select(mm => mm.Menuid)
									.ToList();

								var defaultMenuItems = menuItems
									.Where(mi => defaultMenuIds.Contains(mi.Menuid))
									.ToList();

								foreach (var menuItem in defaultMenuItems)
								{
									var mappingDto = new UserMenuMappingDto
									{
										UserId = newuserId,
										MenuItemId = menuItem.MenuItemId
									};
									await _userMenuMappingService.AddUserMenuMapping(mappingDto);
								}

							}
							ViewData["Message"] = "Employee added successfully.";
							ModelState.Clear();
						}
						else
						{
							ViewData["ErrorMessage"] = "Employee Not Added";
						}
					}
				}
				catch (Exception ex)
				{
					ViewBag.errormsg = ex.Message;
				}
			}
			var roles = await _roleService.GetAll();
			ViewBag.Roles = roles;
			var users = await _userService.GetAllUser();
			ViewBag.Users = users;

			return View(userDto);
		}

		public IActionResult Userview()
		{
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> UserViewDetail()
		{
			if (_globalClass.Token != null)
			{
				var users = await _userService.GetAllUser();
				var roles = await _roleService.GetAll();

				users ??= new List<UserDto>();
				roles ??= new List<RoleDto>();

				var userWithNames = users.Select(u => new
				{
					u.UserId,
					u.Name,
					u.Email,
					u.RoleId,
					roleName = roles.FirstOrDefault(r => r.RoleId == u.RoleId)?.RoleName ?? "Unknown",
					ParentId = u.PerentId,
					ParentName = users.FirstOrDefault(p => p.UserId == u.PerentId)?.Name ?? "N/A"
				}).ToList();

				var roleList = roles.Select(r => new
				{
					id = r.RoleId,
					name = r.RoleName
				}).ToList();

				return Json(new { result = "Success", data = userWithNames, roles = roleList });
			}

			return Json(new { result = "Error", message = "Unauthorized access" });
		}


		public async Task<IActionResult> DeleteUser(int Id)
		{
			try
			{
				if (_globalClass.Token != null)
				{
					var result = await _userService.DeleteUser(Id);
					if (result > 0)
					{
						TempData["Message"] = "User Deleted successfully.";
						return RedirectToAction("Userview");
					}
					else
					{
						TempData["errormsg"] = "User not found.";
						return RedirectToAction("Userview");
					}
				}
				return Json(new { success = false, message = "Unauthorized access." });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "An error occurred: " + ex.Message });
			}

		}

	}
}
