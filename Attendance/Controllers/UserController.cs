using Attendance.Application.Interface;
using Attendance.Application.service;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Mvc;

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
		public UserController(IConfiguration configuration, GlobalClass globalClass, IUserService userService, IUserMenuMappingService userMenuMappingService, IMenuMasterService menuService, IRoleService roleService) : base(menuService, userMenuMappingService)
		{
			_configuration = configuration;
			applicationURL = new ApplicationURL(_configuration);
			_globalClass = globalClass;
			_userService = userService;
			_userMenuMappingService = userMenuMappingService;
			_menuService = menuService;
			_roleService = roleService;
		}
		public IActionResult User()
		{
			var roles = _roleService.GetAll().Result;
			ViewBag.Roles = roles;
			var users = _userService.GetAllUser().Result;
			ViewBag.Users = users;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> User(UserDto userDto)
		{
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
						await _userService.AddUser(new UserDto
						{
							Name = userDto.Name,
							Email = userDto.Email,
							Password = userDto.Password,
							RoleId = userDto.RoleId,
							PerentId = userDto.PerentId,
							IsActive = true,
							CreatedAt = DateTime.Now,
							CreatedBy = 2
						});
						ViewData["Message"] = "User added successfully.";

					}
				}
				catch (Exception ex)
				{
					ViewBag.errormsg = ex.Message;
				}
			}
			var roles = _roleService.GetAll().Result;
			ViewBag.Roles = roles;
			var users = _userService.GetAllUser().Result;
			ViewBag.Users = users;

			return View();
		}

		public IActionResult Userview()
		{
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> UserViewDetail()
		{
			var users = await _userService.GetAllUser();
			var roles = await _roleService.GetAll();

			if (users == null || !users.Any())
			{
				return Json(new { result = "Success", data = new List<object>(), roles = new List<object>() });
			}

			var userWithNames = users.Select(u => new
			{
				u.UserId,
				u.Name,
				u.Email,
				u.RoleId,
				roleName = roles.FirstOrDefault(r => r.RoleId == u.RoleId)?.RoleName,
				ParentId = u.PerentId,
				ParentName = users.FirstOrDefault(p => p.UserId == u.PerentId)?.Name,
				//u.UpdatedBy,
				//u.UpdatedAt
			});

			var roleList = roles.Select(r => new
			{
				id = r.RoleId,
				name = r.RoleName
			});

			return Json(new { result = "Success", data = userWithNames, roles = roleList });
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
