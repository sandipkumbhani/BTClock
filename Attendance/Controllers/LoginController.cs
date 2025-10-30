using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Attendance.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly ILoginServices _loginServices;
        private readonly IConfiguration _configuration;
        private readonly ApplicationURL _applicationURL;

        public LoginController(ILoginServices loginServices, IConfiguration configuration)
        {
            _loginServices = loginServices;
            _configuration = configuration;
            _applicationURL = new ApplicationURL(configuration);

        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(User model, string? returnUrl)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                ViewData["LoginMessage"] = "Email and password are required!";
                return View();
            }

            var tokenString = await _loginServices.Login(model);

            if (tokenString == null || string.IsNullOrEmpty(tokenString.Token))
            {
                ViewData["LoginMessage"] = "Invalid email or password!";
                return View();
            }

            Response.Cookies.Append("jwtToken", tokenString.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddHours(24)
            });

            var menuIds = tokenString.MenuItems?
                 .Select(m => m.MenuItemId)
                 .Distinct()
                 .ToList() ?? new List<int>();

            var menuJson = JsonSerializer.Serialize(menuIds);
            Response.Cookies.Append("MenuAccess", menuJson, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddHours(24)
            });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, tokenString.User.UserId.ToString()),
                new Claim(ClaimTypes.Name, tokenString.User.Name ?? ""),
                new Claim(ClaimTypes.Email, tokenString.User.Email ?? ""),
                new Claim(ClaimTypes.Role, tokenString.User.RoleId.ToString() ?? "0"),
                new Claim("RoleName", tokenString.User.Role?.RoleName ?? ""),
                new Claim("CompanyId", tokenString.User.CompanyId.ToString())
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return string.IsNullOrEmpty(returnUrl)
                ? RedirectToAction("Dashboard", "Dashboard")
                : LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwtToken");
            Response.Cookies.Delete("MenuAccess");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
