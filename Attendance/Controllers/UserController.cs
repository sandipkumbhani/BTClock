using Attendance.Application.Interface;
using Attendance.Application.service;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Attendance.Controllers
{
    [Authorize]
    public class UserController : BaseAdminController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;
        private readonly ApplicationURL _applicationURL;
        private readonly GlobalClass _globalClass;

        public UserController(
            IConfiguration configuration,
            GlobalClass globalClass,
            IUserService userService,
            IRoleService roleService,
            IMenuMasterService menuService,
            IUserMenuMappingService userMenuMappingService,
            IMenuItemService menuItemService
        ) : base(menuService, userMenuMappingService, menuItemService)
        {
            _globalClass = globalClass;
            _userService = userService;
            _roleService = roleService;
            _menuService = menuService;
            _userMenuMappingService = userMenuMappingService;
            _applicationURL = new ApplicationURL(configuration);
        }

        public async Task<IActionResult> AddUser()
        {
            if (_globalClass.Token != null)
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_globalClass.Token);
                var claims = UserUtility.AddClaimsToUser(HttpContext, jwt.Claims);
                string currentPage = "Add User";
                var canAccess = UserUtility.CanAccessMenu(HttpContext, currentPage);

                if (canAccess)
                {
                    var User = claims.Claims.FirstOrDefault(x => x.Type == "UserId");
                    int UserId = User != null ? (!string.IsNullOrEmpty(User.Value) ? Convert.ToInt32(User.Value) : 0) : 0;
                    ViewBag.UserId = UserId;
                    ViewBag.appUrl = _applicationURL.url;
                    ViewBag.Roles = await _roleService.GetAll();
                    ViewBag.Users = await _userService.GetAllUser();
                    return View();
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            return RedirectToAction("Login", "Login");
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.errormsg = "Please fill all required fields.";
                return View(userDto);
            }

            try
            {

                await _userService.AddUser(userDto);
                ViewBag.msg = "User saved successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.errormsg = ex.Message;
            }

            ViewBag.Roles = await _roleService.GetAll();
            ViewBag.Users = await _userService.GetAllUser();
            ViewBag.appUrl = _applicationURL.url;

            return View("AddUser", userDto);
        }
        [HttpGet]
        public async Task<IActionResult> GetRole()
        {
            if (_globalClass.Token != null)
            {
                var result = await _roleService.GetAll();
                return Json(result);
            }
            return Json(new { result = "failure" });
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

            var data = users.Select(u => new
            {
                u.UserId,
                u.Name,
                u.Email,
                u.RoleId,
                RoleName = roles.FirstOrDefault(r => r.RoleId == u.RoleId)?.RoleName ?? "",
                u.ParentId,
                ParentName = users.FirstOrDefault(p => p.UserId == u.ParentId)?.Name ?? "-"
            }).ToList();

            var roleList = roles.Select(r => new { r.RoleId, r.RoleName }).ToList();

            return Json(new { result = "success", data, roles = roleList });
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                TempData["Message"] = result > 0 ? "User deleted successfully." : "User not found.";
                return RedirectToAction("Userview");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
